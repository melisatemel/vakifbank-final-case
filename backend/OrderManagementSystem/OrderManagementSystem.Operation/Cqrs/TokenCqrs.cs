using MediatR;
using OrderManagementSystem.Schema;

namespace OrderManagementSystem.Operation.Cqrs;

public record CreateTokenCommand(TokenRequest Model) : IRequest<ApiResponse<TokenResponse>>;
