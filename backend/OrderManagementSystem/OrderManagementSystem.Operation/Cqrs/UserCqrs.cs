using MediatR;
using OrderManagementSystem.Schema;

namespace OrderManagementSystem.Operation.Cqrs;

public class UserCqrs
{
    public record CreateUserCommand (UserRequest Model) : IRequest<ApiResponse<UserResponse>>;
    public record UpdateUserCommand (UserRequest Model, int Id) : IRequest<ApiResponse>;
    public record DeleteUserCommand (int Id) : IRequest<ApiResponse>;
    public record GetAllUserQuery() : IRequest<ApiResponse<List<UserResponse>>>;
    public record GetUserByIdQuery(int Id) : IRequest<ApiResponse<UserResponse>>;

}
