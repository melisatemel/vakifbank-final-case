using MediatR;
using OrderManagementSystem.Repositories;
using OrderManagementSystem.Schema;
using static OrderManagementSystem.Operation.Cqrs.OrderReportCqrs;
namespace OrderManagementSystem.Operation.Query
{
    public class OrderReportHandler : IRequestHandler<GetOrderReportQuery, IEnumerable<OrderReportDto>>
    {
        private readonly OrderReportRepository _orderReportRepository;

        public OrderReportHandler(OrderReportRepository orderReportRepository)
        {
            _orderReportRepository = orderReportRepository;
        }

        public async Task<IEnumerable<OrderReportDto>> Handle(GetOrderReportQuery request, CancellationToken cancellationToken)
        {
            return await _orderReportRepository.GetOrderReportByUserId(request.Id);
        }
    }
}
