using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Schema;
using static OrderManagementSystem.Operation.Cqrs.ShoppingCartCqrs;

namespace OrderManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShoppingCartController : ControllerBase
{
    private IMediator mediator;

    public ShoppingCartController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("getAllCartItems")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<List<ShoppingCartResponse>>> GetAll()
    {
        var operation = new GetAllShoppingCartQuery();
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("getShoppingCartById/{id}")]
    [Authorize(Roles = "admin, dealer")]
    public async Task<ApiResponse<ShoppingCartResponse>> Get(int id)
    {
        var operation = new GetShoppingCartByIdQuery(id);
        var result = await mediator.Send(operation);
        return result;
    }


    [HttpGet("getComplatedShoppingCartById/{id}")]
    [Authorize(Roles = "admin, dealer")]
    public async Task<ApiResponse<List<ShoppingCartResponse>>> GetComplatedCarts(int id)
    {
        var operation = new GetCompletedShoppingCartsQuery(id);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPost("createShoppingCart")]
    [Authorize(Roles = "admin, dealer")]
    public async Task<ApiResponse<ShoppingCartResponse>> Post([FromBody] ShoppingCartRequest request)
    {
        var operation = new CreateShoppingCartCommand(request);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpDelete("deleteShoppingCart/{id}")]
    [Authorize(Roles = "admin, dealer")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new DeleteShoppingCartCommand(id);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPut("updateShoppingCartById/{id}")]
    [Authorize(Roles = "admin, dealer")]
    public async Task<ApiResponse<ShoppingCartResponse>> Update(int id, [FromBody] ShoppingCartUpdateRequest request)
    {
        var operation = new UpdateShoppingCartByIdCommand(id, request);
        var result = await mediator.Send(operation);
        return result;
    }

}