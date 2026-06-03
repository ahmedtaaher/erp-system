namespace Application.Features.Orders
{
  public record CreateOrderItemDto(
    Guid ProductId,
    int Quantity
  );
}