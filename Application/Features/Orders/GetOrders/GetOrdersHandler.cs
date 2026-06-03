using Application.Common.Interfaces;
using Application.Common.Models;
using Contracts.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.GetOrders
{
  public class GetOrdersHandler : IRequestHandler<GetOrdersQuery, PagedResult<OrderResponse>>
  {
    private readonly IApplicationDbContext _db;

    public GetOrdersHandler(IApplicationDbContext db)
    {
      _db = db;
    }

    public async Task<PagedResult<OrderResponse>> Handle(GetOrdersQuery request, CancellationToken ct)
    {
      var query = _db.Orders.AsNoTracking().Include(x => x.Customer).AsQueryable();

      if (!string.IsNullOrWhiteSpace(request.Search))
      {
        query = query.Where(x => x.Customer.Name.Contains(request.Search));
      }

      query = request.SortBy?.ToLower() switch
      {
        "amount" => request.Descending? query.OrderByDescending(x => x.TotalAmount) : query.OrderBy(x => x.TotalAmount),

        "customer" => request.Descending? query.OrderByDescending(x => x.Customer.Name) : query.OrderBy(x => x.Customer.Name),

        _ => request.Descending? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt)
      };

      var total = await query.CountAsync(ct);

      var orders = await query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).Select(x => new OrderResponse 
        { 
          Id = x.Id, 
          CustomerName = x.Customer.Name, 
          TotalAmount = x.TotalAmount, 
          CreatedAt = x.CreatedAt 
        }).ToListAsync(ct);

      return new PagedResult<OrderResponse>
      {
        Items = orders,
        TotalCount = total,
        Page = request.Page,
        PageSize = request.PageSize
      };
    }
  }
}