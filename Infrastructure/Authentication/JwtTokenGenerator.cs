using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Application.Common.Interfaces.Authentication;
using System.Text;
using Application.Common.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace Infrastructure.Authentication;
public class JwtTokenGenerator : IJwtTokenGenerator
{
  private readonly JwtSettings _jwtSettings;
  private readonly IDateTimeProvider _iDateTimeProvider;

  public JwtTokenGenerator(IDateTimeProvider iDateTimeProvider, IOptions<JwtSettings> jwtOptions)
  {
    _iDateTimeProvider = iDateTimeProvider;
    _jwtSettings = jwtOptions.Value;
  }

  public string GenerateToken(Guid userId, string firstName, string lastName)
  {
    var signingCredentials = new SigningCredentials(
      new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
      SecurityAlgorithms.HmacSha256);
    var claims = new[]
    {
      new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
      new Claim(JwtRegisteredClaimNames.GivenName, firstName),
      new Claim(JwtRegisteredClaimNames.FamilyName, lastName),
      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };
    var securityToken = new JwtSecurityToken(
      issuer: _jwtSettings.Issuer,
      audience: _jwtSettings.Audience,
      expires: _iDateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
      claims: claims,
      signingCredentials: signingCredentials);
    return new JwtSecurityTokenHandler().WriteToken(securityToken);
  }
}