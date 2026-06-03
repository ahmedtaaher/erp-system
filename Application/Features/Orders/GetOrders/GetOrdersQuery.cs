using Application.Common.Models;
using Contracts.Orders;
using MediatR;

namespace Application.Features.Orders.GetOrders
{
  public record GetOrdersQuery(
    int Page = 1,
    int PageSize = 20,
    string? Search = null,
    string? SortBy = null,
    bool Descending = true
  ) : IRequest<PagedResult<OrderResponse>>;
}