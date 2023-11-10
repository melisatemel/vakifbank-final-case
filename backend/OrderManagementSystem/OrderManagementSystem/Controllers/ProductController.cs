using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Schema;
using static OrderManagementSystem.Operation.Cqrs.ProductCqrs;

namespace OrderManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private IMediator mediator;

    public ProductController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("gelAllProducts/{id}")]
    [Authorize(Roles = "admin, dealer")]
    public async Task<ApiResponse<List<ProductResponse>>> GetAll(int id)
    {
        var operation = new GetAllProductQuery(id);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("getProductById/{id}/{userId}")]
    [Authorize(Roles = "admin, dealer")]
    public async Task<ApiResponse<ProductResponse>> Get(int id, int userId)
    {
        var operation = new GetProductByIdQuery(id, userId);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPost("createProduct")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<ProductResponse>> Post([FromBody] ProductRequest request)
    {
        var operation = new CreateProductCommand(request);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPut("updateProduct/{id}")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse> Put(int id, [FromBody] ProductRequest request)
    {
        var operation = new UpdateProductCommand(request, id);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpDelete("deleteProduct/{id}")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new DeleteProductCommand(id);
        var result = await mediator.Send(operation);
        return result;
    }
}
