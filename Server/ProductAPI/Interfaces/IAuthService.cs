public interface IAuthService
{
    string AuthenticateUser(string username, string password);
    string GetUserRole(string username);
}
