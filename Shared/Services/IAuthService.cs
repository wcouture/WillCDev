namespace Shared.Services
{
    public interface IAuthService
    {
        Task<bool> IsAuthenticated();
        Task<string> GetUsername();
        Task<bool> Login(string username, string password);
        Task<bool> Register(string username, string password);
        Task Logout();
    }
}