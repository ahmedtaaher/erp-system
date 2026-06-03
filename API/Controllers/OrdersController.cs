using Application.Features.Orders;
using Application.Features.Orders.GetOrders;
using Application.Features.Orders.UpdateOrder;
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

    [HttpGet]
    public async Task<IResult> Get([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string? search, [FromQuery] string? sortBy, [FromQuery] bool descending, IMediator mediator)
    {
      var result = await mediator.Send(new GetOrdersQuery(page, pageSize, search, sortBy, descending));

      return Results.Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetById(Guid id, IMediator mediator)
    {
      var result = await mediator.Send(new GetOrderByIdQuery(id));

      return Results.Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IResult> Update(Guid id, UpdateOrderRequest request, IMediator mediator)
    {
      await mediator.Send(new UpdateOrderCommand(id, request.Version, request.Items.Select(x => new UpdateOrderItemDto(x.ProductId, x.Quantity)).ToList()));

      return Results.Ok();
    }
  }
}