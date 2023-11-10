using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Operation;
using OrderManagementSystem.Schema;

namespace OrderManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessagesController : ControllerBase
{
    private readonly IMediator mediator;

    public MessagesController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("getAllMessages")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<List<MessageResponse>>> GetAll()
    {
        var operation = new GetAllMessageQuery();
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("getMessageById/{id}")]
    [Authorize(Roles = "admin, dealer")]
    public async Task<ApiResponse<List<MessageResponse>>> Get(int id)
    {
        var operation = new GetMessageByIdQuery(id);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPost("createMessage")]
    [Authorize(Roles = "admin, dealer")]
    public async Task<ApiResponse<MessageResponse>> Post([FromBody] MessageRequest request)
    {
        var operation = new CreateMessageCommand(request);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPut("updateMessage/{id}")]
    [Authorize(Roles = "admin, dealer")]
    public async Task<ApiResponse> Put(int id, [FromBody] MessageRequest request)
    {
        var operation = new UpdateMessageCommand(request, id);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpDelete("deleteMessage/{id}")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new DeleteMessageCommand(id);
        var result = await mediator.Send(operation);
        return result;
    }


    [HttpGet("getAdminMessages")]
    [Authorize(Roles = "admin, dealer")]
    public async Task<ApiResponse<List<AdminMessageResponse>>> GetAdminMessages()
    {
        try
        {
            var operation = new GetAdminMessagesQuery();
            var result = await mediator.Send(operation);

            return result;
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<AdminMessageResponse>>($"Error: {ex.Message}");
        }
    }
}