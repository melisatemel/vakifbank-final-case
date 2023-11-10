using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Operation.Cqrs;
using OrderManagementSystem.Schema;

namespace OrderManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokenContoller : ControllerBase
{
    private IMediator mediator;

    public TokenContoller(IMediator mediator)
    {
        this.mediator = mediator;
    }


    [HttpPost]
    public async Task<ApiResponse<TokenResponse>> Post([FromBody] TokenRequest request)
    {
        var operation = new CreateTokenCommand(request);
        var result = await mediator.Send(operation);
        return result;
    }


}