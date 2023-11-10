using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Operation;
using OrderManagementSystem.Schema;

namespace OrderManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AddressesController : ControllerBase
{
    private IMediator mediator;

    public AddressesController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("getAllAddresses")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<List<AddressResponse>>> GetAll()
    {
        var operation = new GetAllAddressQuery();
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("getAddressById/{id}")]
    [Authorize(Roles = "admin, dealer")]
    public async Task<ApiResponse<AddressResponse>> Get(int id)
    {
        var operation = new GetAddressByIdQuery(id);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPost("createAddress")]
    [Authorize(Roles = "admin, dealer")]
    public async Task<ApiResponse<AddressResponse>> Post([FromBody] AddressRequest request)
    {
        var operation = new CreateAddressCommand(request);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPut("updateAddressById/{id}")]
    [Authorize(Roles = "admin, dealer")]
    public async Task<ApiResponse> Put(int id, [FromBody] AddressRequest request)
    {
        var operation = new UpdateAddressCommand(request, id);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpDelete("deleteAddress/{id}")]
    [Authorize(Roles = "admin, dealer")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new DeleteAddressCommand(id);
        var result = await mediator.Send(operation);
        return result;
    }
}
