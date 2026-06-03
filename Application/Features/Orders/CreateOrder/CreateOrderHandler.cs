using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.CreateOrder
{
  public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Guid>
  {
    private readonly IApplicationDbContext _db;

    public CreateOrderHandler(IApplicationDbContext db)
    {
      _db = db;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken ct)
    {
      var customer = await _db.Customers.FirstOrDefaultAsync(x => x.Id == request.CustomerId, ct);

        if (customer is null)
          throw new Exception("Customer not found");

      var order = new Order
      {
        CustomerId = request.CustomerId
      };

      decimal total = 0;

      foreach (var item in request.Items)
      {
        var product = await _db.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId, ct);

        if (product is null)
          throw new Exception($"Product {item.ProductId} not found");

        if (product.StockQuantity < item.Quantity)
          throw new Exception($"Insufficient stock for {product.Name}");

         product.StockQuantity -= item.Quantity;

        var orderItem = new OrderItem
        {
          ProductId = product.Id,
          Quantity = item.Quantity,
          UnitPrice = product.Price
        };

        total += product.Price * item.Quantity;

        order.Items.Add(orderItem);
      }

      order.TotalAmount = total;

      _db.Orders.Add(order);

      await _db.SaveChangesAsync(ct);

      return order.Id;
    }
  }
}