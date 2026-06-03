using Application.Common.Interfaces;
using Domain.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Persistence
{
  public class TenantProvider : ITenantProvider
  {
    private readonly ICurrentUser _currentUser;

    public TenantProvider(ICurrentUser currentUser)
    {
      _currentUser = currentUser;
    }

    public Guid TenantId => _currentUser.TenantId;
  }
}