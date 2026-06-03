using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.GetOrderById
{
  public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderDetailsResponse>
  {
    private readonly IApplicationDbContext _db;

    public GetOrderByIdHandler(IApplicationDbContext db)
    {
      _db = db;
    }

    public async Task<OrderDetailsResponse> Handle(GetOrderByIdQuery request, CancellationToken ct)
    {
      var order = await _db.Orders.AsNoTracking().Include(x => x.Customer).Include(x => x.Items).ThenInclude(x => x.Product).FirstOrDefaultAsync(x => x.Id == request.OrderId, ct);

      if (order is null)
        throw new Exception("Order not found");

      return new OrderDetailsResponse
      {
        Id = order.Id,

        Version = order.Version,

        CustomerName = order.Customer.Name,

        TotalAmount = order.TotalAmount,

        CreatedAt = order.CreatedAt,

        Items = order.Items.Select(x => new OrderItemResponse
        {
          ProductId = x.ProductId,
          ProductName = x.Product.Name,
          Quantity = x.Quantity, 
          UnitPrice = x.UnitPrice
        }).ToList()
      };
    }
  }
}