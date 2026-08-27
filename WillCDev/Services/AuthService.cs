using Microsoft.AspNetCore.Components.Authorization;
using Shared.Attributes;
using Shared.Services;
using System.Threading.Tasks;

namespace WillCDev.Services
{
    [Service(typeof(IAuthService), ServiceType.Scoped)]
    public class AuthService(AuthenticationStateProvider authenticationStateProvider) : IAuthService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;

        public async Task<bool> IsAuthenticated()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return authState.User.Identity.IsAuthenticated;
        }

        public Task SignIn(string username, string password)
        {
            // Custom sign-in logic can be added here.
            return Task.FromResult(true);
        }

        public Task SignOut()
        {
            // Custom sign-out logic can be added here.
            return Task.CompletedTask;
        }
    }
}