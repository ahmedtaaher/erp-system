using MediatR;

namespace Application.Features.Orders.UpdateOrder
{
  public record UpdateOrderCommand(
    Guid OrderId,
    uint Version,
    List<UpdateOrderItemDto> Items
  ) : IRequest;
}