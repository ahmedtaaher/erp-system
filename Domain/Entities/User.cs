using Domain.Common;

namespace Domain.Entities
{
  public class User : BaseTenantEntity
  {
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public Tenant Tenant { get; set; } = null!;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "Employee";
  }
}