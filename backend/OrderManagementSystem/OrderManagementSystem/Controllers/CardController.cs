using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Operation;
using OrderManagementSystem.Schema;

namespace OrderManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private IMediator mediator;

        public CardsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("getAllCards")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<List<CardResponse>>> GetAll()
        {
            var operation = new GetAllCardQuery();
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpGet("getCardById/{id}")]
        [Authorize(Roles = "admin, dealer")]
        public async Task<ApiResponse<CardResponse>> Get(int id)
        {
            var operation = new GetCardByIdQuery(id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPost("createCard")]
        [Authorize(Roles = "admin, dealer")]
        public async Task<ApiResponse<CardResponse>> Post([FromBody] CardRequest request)
        {
            var operation = new CreateCardCommand(request);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPut("updateCard/{id}")]
        [Authorize(Roles = "admin, dealer")]
        public async Task<ApiResponse> Put(int id, [FromBody] CardRequest request)
        {
            var operation = new UpdateCardCommand(request, id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpDelete("deleteCard/{id}")]
        [Authorize(Roles = "admin, dealer")]
        public async Task<ApiResponse> Delete(int id)
        {
            var operation = new DeleteCardCommand(id);
            var result = await mediator.Send(operation);
            return result;
        }
    }
}
