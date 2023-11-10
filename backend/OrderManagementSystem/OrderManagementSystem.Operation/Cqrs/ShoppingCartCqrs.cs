using MediatR;
using OrderManagementSystem.Schema;

namespace OrderManagementSystem.Operation.Cqrs;

public class ShoppingCartCqrs
{
    public record CreateShoppingCartCommand(ShoppingCartRequest Model) : IRequest<ApiResponse<ShoppingCartResponse>>;
    public record DeleteShoppingCartCommand(int Id) : IRequest<ApiResponse>;
    public record GetAllShoppingCartQuery() : IRequest<ApiResponse<List<ShoppingCartResponse>>>;
    public record GetShoppingCartByIdQuery(int Id) : IRequest<ApiResponse<ShoppingCartResponse>>;
    public record GetCompletedShoppingCartsQuery(int Id) : IRequest<ApiResponse<List<ShoppingCartResponse>>>;
    public record UpdateShoppingCartByIdCommand(int Id, ShoppingCartUpdateRequest Model) : IRequest<ApiResponse<ShoppingCartResponse>>;


}
