namespace Shared.Services
{
    public interface IAuthService
    {
        Task<bool> IsAuthenticated(Guid guid);
        Task<string> GetUsername(Guid guid);
        Task<Guid> SignIn(string username, string password);
        Task SignOut(Guid guid);
    }
}