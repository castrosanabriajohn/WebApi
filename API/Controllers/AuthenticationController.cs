using Application.Services;
using Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
  private readonly IAuthenticationService _iAuthenticationService;

  public AuthenticationController(IAuthenticationService IauthenticationService)
  {
    _iAuthenticationService = IauthenticationService;
  }

  [HttpPost("register")]
  public IActionResult Register(RegisterRequest request) // Receives contract parameters
  { // Creates new authentication result via interface passing the contract parameters
    var authResult = _iAuthenticationService.Register(
      request.FirstName,
      request.LastName,
      request.Email,
      request.Password);
    // Maps a new authentication response contract based on the result object passed 
    var response = new AuthenticationResponse(
      authResult.Id,
      authResult.FirstName,
      authResult.LastName,
      authResult.Email,
      authResult.Token
    );
    return Ok(response);
  }

  [HttpPost("login")]
  public IActionResult Login(LoginRequest request)
  {
    var authResult = _iAuthenticationService.Login(
      request.Email,
      request.Password);
    var response = new AuthenticationResponse(
      authResult.Id,
      authResult.FirstName,
      authResult.LastName,
      authResult.Email,
      authResult.Token
    );
    return Ok(response);
  }
}