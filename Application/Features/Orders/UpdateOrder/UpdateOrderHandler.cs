using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.UpdateOrder
{
  public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand>
  {
    private readonly IApplicationDbContext _db;

    public UpdateOrderHandler(IApplicationDbContext db)
    {
      _db = db;
    }

    public async Task Handle(UpdateOrderCommand request, CancellationToken ct)
    {
      await _db.BeginTransactionAsync(ct);

      try
      {
        var order = await _db.Orders.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == request.OrderId, ct);
        
        if (order is null)
          throw new Exception("Order not found");
        
        _db.Entry(order).Property(nameof(order.Version)).OriginalValue = request.Version;

        foreach (var oldItem in order.Items)
        {
          var product = await _db.Products.FirstAsync(x => x.Id == oldItem.ProductId, ct);

          product.StockQuantity += oldItem.Quantity;
        }

        _db.OrderItems.RemoveRange(order.Items);

        decimal total = 0;

        foreach (var item in request.Items)
        {
          var product = await _db.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId, ct);

          if (product is null)
            throw new Exception("Product not found");

          if (product.StockQuantity < item.Quantity)
          {
            throw new Exception("Insufficient stock");
          }

          product.StockQuantity -= item.Quantity;

          order.Items.Add(new Domain.Entities.OrderItem
          {
            ProductId = product.Id,

            Quantity = item.Quantity,

            UnitPrice = product.Price
          });

          total += product.Price * item.Quantity;
        }

        order.TotalAmount = total;

        await _db.SaveChangesAsync(ct);

        await _db.CommitTransactionAsync(ct);
      }

      catch (DbUpdateConcurrencyException)
      {
        await _db.RollbackTransactionAsync(ct);

        throw new Exception("Order was modified by another user");
      }

      catch
      {
        await _db.RollbackTransactionAsync(ct);

        throw;
      }
    }
  }
}