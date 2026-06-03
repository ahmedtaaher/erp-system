using Domain.Common;

namespace Domain.Entities
{
  public class Product : BaseTenantEntity
  {
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public ICollection<OrderItem> OrderItems = new List<OrderItem>();
  }
}