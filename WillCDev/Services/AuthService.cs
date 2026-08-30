using Microsoft.AspNetCore.Components.Authorization;
using Shared.Attributes;
using Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using Shared.Services;

namespace WillCDev.Services
{
    [Service(typeof(IAuthService), ServiceType.Scoped)]
    public class AuthService(AuthenticationStateProvider authenticationStateProvider, IHttpClientFactory httpClientFactory) : IAuthService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public async Task<bool> IsAuthenticated()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            return user.Identity?.IsAuthenticated ?? false;
        }

        public async Task<bool> Login(string username, string password)
        {
            // Custom sign-in logic can be added here.
            try
            {
                using (var httpClient = _httpClientFactory.CreateClient("Self"))
                {
                    var response = await httpClient.PostAsJsonAsync("/api/auth/login", new { username, password });
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception("Login failed.");
                    }

                    var content = await response.Content.ReadFromJsonAsync<LoginResult>();
                    if (content == null || content.Token == null)
                    {
                        throw new Exception("Login failed: Invalid token.");
                    }

                    await ((JwtAuthenticationStateProvider)_authenticationStateProvider).SetUserAsAuthenticated(content.Token);

                }
                
                return true;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the login process.
                Console.WriteLine($"Login failed: {ex.Message}");
                return false;
            }
        }

        public async Task Logout()
        {
            await ((JwtAuthenticationStateProvider)_authenticationStateProvider).NotifyUserLogout();
        }

        public async Task<string> GetUsername()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            return user.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName)?.Value ?? string.Empty;
        }
    }
}