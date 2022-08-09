using Domain.Entities;

namespace Application.Services;
public record AuthenticationResult(
  User User,
  string Token
);