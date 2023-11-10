using MediatR;
using OrderManagementSystem.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Operation
{
    public record CreateMessageCommand(MessageRequest Model) : IRequest<ApiResponse<MessageResponse>>;
    public record UpdateMessageCommand(MessageRequest Model, int Id) : IRequest<ApiResponse>;
    public record DeleteMessageCommand(int Id) : IRequest<ApiResponse>;
    public record GetAllMessageQuery() : IRequest<ApiResponse<List<MessageResponse>>>;
    public record GetMessageByIdQuery(int Id) : IRequest<ApiResponse<List<MessageResponse>>>;
}