namespace Contracts.Orders
{
  public class UpdateOrderRequest
  {
    public uint Version { get; set; }
    public List<UpdateOrderItemRequest> Items { get; set; } = [];
  }
}