using AutoMapper;
using MediatR;
using OrderManagementSystem.Data.Context;
using OrderManagementSystem.Data.Domain;
using OrderManagementSystem.Schema;
using Microsoft.EntityFrameworkCore;
using static OrderManagementSystem.Operation.Cqrs.ProductCqrs;
using Microsoft.AspNetCore.Http;

namespace OrderManagementSystem.Operation.Query;

public class ProductQueryHandler :

    IRequestHandler<GetAllProductQuery, ApiResponse<List<ProductResponse>>>,
    IRequestHandler<GetProductByIdQuery, ApiResponse<ProductResponse>>
{

    private readonly OmsDbContext dbContext;
    private readonly IMapper mapper;

    public ProductQueryHandler(OmsDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<ProductResponse>>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
    {
        List<Product> list = await dbContext.Set<Product>().Where(x => x.IsActive)
            .ToListAsync(cancellationToken);

        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == request.id);
        var userProfitMargin = user?.ProfitMargin ?? 0; 

        if (user != null && user.Role != "admin")
        {
            foreach (var product in list)
            {
                if (product != null)
                {
                    product.Price += product.Price * userProfitMargin / 100;
                    product.Price += product.Price * 0.20m; 
                }
            }
        }

        List<ProductResponse> mapped = mapper.Map<List<ProductResponse>>(list);
        return new ApiResponse<List<ProductResponse>>(mapped);
    }


    public async Task<ApiResponse<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        Product entity = await dbContext.Set<Product>()
            .FirstOrDefaultAsync(x => x.ProductId == request.Id, cancellationToken);

        if (entity == null)
        {
            return new ApiResponse<ProductResponse>("Record not found!" + request);
        }

        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId);
        var userProfitMargin = user?.ProfitMargin ?? 0;

        if (user != null && user.Role != "admin")
        {
            entity.Price += entity.Price * userProfitMargin / 100;
            entity.Price += entity.Price * 0.20m;
        }

        ProductResponse mapped = mapper.Map<ProductResponse>(entity);
        return new ApiResponse<ProductResponse>(mapped);
    }


}