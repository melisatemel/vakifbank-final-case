using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Schema;
using static OrderManagementSystem.Operation.Cqrs.OrderReportCqrs;

namespace OrderManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderReportController : ControllerBase
    {
        private IMediator mediator;

        public OrderReportController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("getOrderReport/{id}")]
        [Authorize(Roles = "admin, dealer")]
        public async Task<ApiResponse<IEnumerable<OrderReportDto>>> GetOrderReport(int id)
        {
            var query = new GetOrderReportQuery(id);
            var result = await mediator.Send(query);
            return new ApiResponse<IEnumerable<OrderReportDto>>(result);
        }
    }
}
