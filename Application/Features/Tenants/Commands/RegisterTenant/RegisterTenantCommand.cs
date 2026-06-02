using MediatR;

namespace Application.Features.Tenants.Commands.RegisterTenant
{
  public record RegisterTenantCommand(
    string CompanyName,
    string FirstName,
    string LastName,
    string Email,
    string Password
  ) : IRequest<Guid>;
}