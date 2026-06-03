using Application.Features.Orders;
using Contracts.Orders;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class OrdersController : ControllerBase
  {
    [HttpPost]
    public async Task<IResult> Create(CreateOrderRequest request, IMediator mediator)
    {
      var command = new CreateOrderCommand(request.CustomerId, request.Items.Select(x => new CreateOrderItemDto(x.ProductId, x.Quantity)).ToList());

      var id = await mediator.Send(command);

      return Results.Ok(id);
    }
  }
}