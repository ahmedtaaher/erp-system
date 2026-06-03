using Domain.Common;

namespace Domain.Entities
{
  public class Order : BaseTenantEntity
  {
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public ICollection<OrderItem> Items = new List<OrderItem>();
  }
}