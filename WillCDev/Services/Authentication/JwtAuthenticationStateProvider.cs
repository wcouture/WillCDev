using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Shared.Attributes;
using System.Text.Json;
namespace WillCDev.Services
{
    [Service(typeof(AuthenticationStateProvider), ServiceType.Scoped)]
    public class JwtAuthenticationStateProvider(IJSRuntime jsRuntime) : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime = jsRuntime;
        private readonly ClaimsPrincipal _anonynmous = new ClaimsPrincipal(new ClaimsIdentity());

        public async Task SetUserAsAuthenticated(string token)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
            await NotifyUserAuthentication(token);
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
                if (string.IsNullOrWhiteSpace(token))
                {
                    return new AuthenticationState(_anonynmous);
                }

                var claims = ParseClaimsFromJwt(token);
                var identity = new ClaimsIdentity(claims, "jwt");
                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            catch
            {
                return new AuthenticationState(_anonynmous);
            }
        }

        public async Task NotifyUserAuthentication(string token)
        {
            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt", nameType: ClaimTypes.Name, roleType: ClaimTypes.Role);
            var user = new ClaimsPrincipal(identity);
            var authState = Task.FromResult(new AuthenticationState(user));

            NotifyAuthenticationStateChanged(authState);
        }

        public async Task NotifyUserLogout()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");

            var authState = Task.FromResult(new AuthenticationState(_anonynmous));
            NotifyAuthenticationStateChanged(authState);
        }
    
        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(jwt))
            {
                return Array.Empty<Claim>();
            }

            var token = handler.ReadJwtToken(jwt);
            return token.Claims;
        }
    }
}

