using MediatR;

namespace Application.Features.Orders
{
  public record CreateOrderCommand(
    Guid CustomerId,
    List<CreateOrderItemDto> Items
  ) : IRequest<Guid>;
}