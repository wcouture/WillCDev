namespace Shared.Services
{
    public interface IAuthService
    {
        Task<bool> IsAuthenticated();
        Task SignIn(string username, string password);
        Task SignOut();
    }
}