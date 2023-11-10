using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Schema;
using static OrderManagementSystem.Operation.Cqrs.UserCqrs;

namespace OrderManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private IMediator mediator;

    public UserController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("getAllUsers")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<List<UserResponse>>> GetAll()
    {
        var operation = new GetAllUserQuery();
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("getUserById/{id}")]
    [Authorize(Roles = "admin, dealer")]
    public async Task<ApiResponse<UserResponse>> Get(int id)
    {
        var operation = new GetUserByIdQuery(id);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPost("createUser")]
    public async Task<ApiResponse<UserResponse>> Post([FromBody] UserRequest request)
    {
        var operation = new CreateUserCommand(request);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPut("updateUser/{id}")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse> Put(int id, [FromBody] UserRequest request)
    {
        var operation = new UpdateUserCommand(request, id);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpDelete("deleteUser/{id}")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new DeleteUserCommand(id);
        var result = await mediator.Send(operation);
        return result;
    }
}
