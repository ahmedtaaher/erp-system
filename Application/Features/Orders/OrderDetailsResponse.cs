namespace Application.Features.Orders
{
  public class OrderDetailsResponse
  {
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public uint Version { get; set; }
    public List<OrderItemResponse> Items { get; set; } = [];
  }
}