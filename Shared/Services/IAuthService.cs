namespace Shared.Services
{
    public interface IAuthService
    {
        bool IsAuthenticated();
        void SignIn(string username, string password);
        void SignOut();
    }
}