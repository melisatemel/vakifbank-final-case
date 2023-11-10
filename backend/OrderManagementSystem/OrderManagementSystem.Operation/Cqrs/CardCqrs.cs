using MediatR;
using OrderManagementSystem.Schema;

namespace OrderManagementSystem.Operation
{
    public record CreateCardCommand(CardRequest Model) : IRequest<ApiResponse<CardResponse>>;
    public record UpdateCardCommand(CardRequest Model, int Id) : IRequest<ApiResponse>;
    public record DeleteCardCommand(int Id) : IRequest<ApiResponse>;
    public record GetAllCardQuery() : IRequest<ApiResponse<List<CardResponse>>>;
    public record GetCardByIdQuery(int Id) : IRequest<ApiResponse<CardResponse>>;
    public record GetCardByUserIdQuery(int UserId) : IRequest<ApiResponse<List<CardResponse>>>;
    public record GetAdminMessagesQuery() : IRequest<ApiResponse<List<AdminMessageResponse>>>;
}
