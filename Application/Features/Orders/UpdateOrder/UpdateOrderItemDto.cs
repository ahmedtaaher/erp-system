namespace Application.Features.Orders.UpdateOrder
{
  public record UpdateOrderItemDto(
    Guid ProductId,
    int Quantity
  );
}