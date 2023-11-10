using MediatR;
using OrderManagementSystem.Schema;

namespace OrderManagementSystem.Operation.Cqrs;

public class ProductCqrs
{
    public record CreateProductCommand(ProductRequest Model) : IRequest<ApiResponse<ProductResponse>>;
    public record UpdateProductCommand(ProductRequest Model, int Id) : IRequest<ApiResponse>;
    public record DeleteProductCommand(int Id) : IRequest<ApiResponse>;
    public record GetAllProductQuery(int id) : IRequest<ApiResponse<List<ProductResponse>>>;
    public record GetProductByIdQuery(int Id, int UserId) : IRequest<ApiResponse<ProductResponse>>;

}
