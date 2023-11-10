using AutoMapper;
using MediatR;
using OrderManagementSystem.Data.Context;
using OrderManagementSystem.Data.Domain;
using OrderManagementSystem.Schema;
using static OrderManagementSystem.Operation.Cqrs.ProductCqrs;
using Microsoft.EntityFrameworkCore;

namespace OrderManagementSystem.Operation.Command;

public class ProductCommandHandler :
    IRequestHandler<CreateProductCommand, ApiResponse<ProductResponse>>,
    IRequestHandler<UpdateProductCommand, ApiResponse>,
    IRequestHandler<DeleteProductCommand, ApiResponse>
{
    private readonly OmsDbContext dbContext;
    private readonly IMapper mapper;

    public ProductCommandHandler(OmsDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<ProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        int productId = GenerateProductId();

        request.Model.ProductId = productId;
        Product mapped = mapper.Map<Product>(request.Model);

        var entity = await dbContext.Set<Product>().AddAsync(mapped, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = mapper.Map<ProductResponse>(entity.Entity);
        return new ApiResponse<ProductResponse>(response);
    }

    public async Task<ApiResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Set<Product>().FirstOrDefaultAsync(x => x.ProductId == request.Id, cancellationToken);
        if (entity == null)
        {
            return new ApiResponse("Record not found!");
        }
        entity.Name = request.Model.Name;
        entity.Description = request.Model.Description;
        entity.Price = request.Model.Price;
        entity.StockQuantity = request.Model.StockQuantity;
        entity.Image = request.Model.Image;

        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Set<Product>().FirstOrDefaultAsync(x => x.ProductId == request.Id, cancellationToken);
        if (entity == null)
        {
            return new ApiResponse("Record not found!");
        }

        entity.IsActive = false;
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    private int GenerateProductId()
    {
        int maxProductId = dbContext.Set<Product>().Max(p => p.ProductId);
        return maxProductId + 1;
    }

}