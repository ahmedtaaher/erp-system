namespace Contracts.Orders
{
  public class UpdateOrderItemRequest
  {
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }
  }
}