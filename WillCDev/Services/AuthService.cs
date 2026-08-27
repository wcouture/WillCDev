using Microsoft.AspNetCore.Components.Authorization;
using Shared.Attributes;
using Shared.Services;

namespace WillCDev.Services
{
    [Service(typeof(IAuthService), ServiceType.Singleton)]
    public class AuthService : IAuthService
    {
        private Dictionary<Guid, string> _authenticatedUsers { get; set; } = new(); 

        public async Task<bool> IsAuthenticated(Guid guid)
        {
            return _authenticatedUsers.ContainsKey(guid);
        }

        public Task<Guid> SignIn(string username, string password)
        {
            // Custom sign-in logic can be added here.
            var userId = Guid.NewGuid();
            _authenticatedUsers[userId] = username;
            return Task.FromResult(userId);
        }

        public Task SignOut(Guid guid)
        {
            if (_authenticatedUsers.ContainsKey(guid)) {
                _authenticatedUsers.Remove(guid);
            }
            return Task.CompletedTask;
        }

        public Task<string> GetUsername(Guid guid)
        {
            // Return the username associated with the specified GUID, if any.
            var username = _authenticatedUsers.TryGetValue(guid, out var name) ? name : string.Empty;
            return Task.FromResult(username);
        }
    }
}