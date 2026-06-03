using MediatR;

namespace Application.Features.Auth.Login
{
  public record LoginCommand(
    string Email,
    string Password,
    Guid TenantId
  ) : IRequest<string>;
}