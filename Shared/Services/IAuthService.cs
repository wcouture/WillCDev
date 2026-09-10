using System.Net;

namespace Shared.Services
{
    public interface IAuthService
    {
        Task<bool> IsAuthenticated();
        Task<string> GetUsername();
        Task<HttpStatusCode> Login(string username, string password);
        Task<HttpStatusCode> Register(string username, string password);
        Task Logout();
    }
}