using Microsoft.AspNetCore.Components.Authorization;
using Shared.Attributes;
using Shared.Models.Authentication;
using System.IdentityModel.Tokens.Jwt;
using Shared.Services;
using System.Text.Json;
using System.Net;

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

        public async Task<HttpStatusCode> Login(string username, string password)
        {
            // Custom sign-in logic can be added here.
            try
            {
                using (var httpClient = _httpClientFactory.CreateClient("Self"))
                {
                    var response = await httpClient.PostAsJsonAsync("/api/auth/login", new { username, password });

                    var content = await response.Content.ReadFromJsonAsync<LoginResult>();
                    if (content == null)
                        throw new Exception("Bad LoginResult returned from /api/auth/login.");

                    if (response.IsSuccessStatusCode)
                        await ((JwtAuthenticationStateProvider)_authenticationStateProvider).SetUserAsAuthenticated(content.Token!);

                    return response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                return HttpStatusCode.InternalServerError;
            }
        }

        public async Task<HttpStatusCode> Register(string username, string password)
        {
            try
            {
                using (var httpClient = _httpClientFactory.CreateClient("Self"))
                {
                    var response = await httpClient.PostAsJsonAsync("/api/auth/register", new { username, password });

                    var content = await response.Content.ReadFromJsonAsync<RegisterResult>();
                    if (content == null)
                        throw new Exception("Bad RegisterResult returned from /api/auth/register.");

                    return response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                return HttpStatusCode.InternalServerError;
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