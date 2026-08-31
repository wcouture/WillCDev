namespace Shared.Models.Authentication
{
    public class RegisterResult
    {
        public bool Success { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}