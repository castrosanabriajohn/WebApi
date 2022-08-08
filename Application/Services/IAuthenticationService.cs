namespace Application.Services;
public interface IAuthenticationService
{
  AuthenticationResult Register(
    string firstName,
    string lastName,
    string email,
    string password);
  AuthenticationResult Login(
    string Email,
    string Password
  );
}