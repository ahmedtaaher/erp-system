namespace Application.Common.Interfaces
{
  public interface ICurrentUser
  {
    Guid UserId { get; }
    Guid TenantId { get; }
    string Email { get; }
    bool IsAuthenticated { get; }
  }
}