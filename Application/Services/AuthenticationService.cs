using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.Persistence;
using Domain.Entities;

namespace Application.Services;
public class AuthenticationService : IAuthenticationService
{
  private readonly IJwtTokenGenerator _jwtTokenGenerator;
  private readonly IUserRepository _iUserRepository;

  public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository iUserRepository)
  {
    _jwtTokenGenerator = jwtTokenGenerator;
    _iUserRepository = iUserRepository;
  }

  public AuthenticationResult Register(string firstName, string lastName, string email, string password)
  {
    // Vallidate user doesn't exist
    if (_iUserRepository.GetUserByEmail(email) is not null)
    {
      throw new Exception("User with email: " + email + " already registered");
    }
    // Create an user (Generate unique identifier)
    var user = new User
    {
      FirstName = firstName,
      LastName = lastName,
      Email = email,
      Password = password
    };
    // Persist it to database
    _iUserRepository.Add(user);
    // Create JWT Token
    var token = _jwtTokenGenerator.GenerateToken(user.Id, firstName, lastName);
    return new AuthenticationResult(
      user.Id,
      firstName,
      lastName,
      email,
      token);
  }

  public AuthenticationResult Login(string email, string password)
  {
    // Validate the user exists
    if (_iUserRepository.GetUserByEmail(email) is not User user)
    {
      throw new Exception("The user with given email " + email + " does not exist");
    }
    // Validate the password is correct
    if (user.Password != password)
    {
      throw new Exception("Invalid password");
    }
    // Create the JWT token and return it to the user
    var token = _jwtTokenGenerator.GenerateToken(
      user.Id,
      user.FirstName,
      user.LastName);
    return new AuthenticationResult(
      user.Id,
      user.FirstName,
      user.LastName,
      user.Email,
      token);
  }
}