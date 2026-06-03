using MediatR;

namespace Application.Features.Orders
{
  public record GetOrderByIdQuery(
    Guid OrderId
  ) : IRequest<OrderDetailsResponse>;
}