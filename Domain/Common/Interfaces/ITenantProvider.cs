namespace Domain.Common.Interfaces
{
  public interface ITenantProvider
  {
    Guid TenantId { get; }
  }
}