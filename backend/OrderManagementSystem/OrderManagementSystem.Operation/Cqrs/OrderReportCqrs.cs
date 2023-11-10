using MediatR;
using OrderManagementSystem.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Operation.Cqrs
{
    public class OrderReportCqrs
    {
        public record GetOrderReportQuery(int Id) : IRequest<IEnumerable<OrderReportDto>>;

    }
}
