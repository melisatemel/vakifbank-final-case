using AutoMapper;
using MediatR;
using OrderManagementSystem.Data.Context;
using OrderManagementSystem.Data.Domain;
using OrderManagementSystem.Schema;
using Microsoft.EntityFrameworkCore;
using static OrderManagementSystem.Operation.Cqrs.ShoppingCartCqrs;
using OrderManagementSystem.Repositories;
using System.Collections.Generic;

namespace OrderManagementSystem.Operation.Query;

public class ShoppingCartQueryHandler :

    IRequestHandler<GetAllShoppingCartQuery, ApiResponse<List<ShoppingCartResponse>>>,
    IRequestHandler<GetShoppingCartByIdQuery, ApiResponse<ShoppingCartResponse>>,
    IRequestHandler<GetCompletedShoppingCartsQuery, ApiResponse<List<ShoppingCartResponse>>>
{

    private readonly OmsDbContext dbContext;
    private readonly IMapper mapper;

    public ShoppingCartQueryHandler(OmsDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<ShoppingCartResponse>>> Handle(GetAllShoppingCartQuery request, CancellationToken cancellationToken)
    {
        List<ShoppingCart> list = await dbContext.Set<ShoppingCart>()
            .Include(cart => cart.Products).Where(x => x.IsCompleted)
            .ToListAsync(cancellationToken);

        List<ShoppingCartResponse> mapped = new List<ShoppingCartResponse>();

        foreach (var cart in list)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == cart.UserId);
            var userEmail = user != null ? user.Email : "Unknown"; 

            var cartResponse = mapper.Map<ShoppingCartResponse>(cart);
            cartResponse.Email = userEmail;
            mapped.Add(cartResponse);
        }
        return new ApiResponse<List<ShoppingCartResponse>>(mapped);
    }

    public async Task<ApiResponse<ShoppingCartResponse>> Handle(GetShoppingCartByIdQuery request, CancellationToken cancellationToken)
    {
        ShoppingCart entity = await dbContext.Set<ShoppingCart>()
            .Include(cart => cart.Products)
            .FirstOrDefaultAsync(x => x.UserId == request.Id && !x.IsCompleted, cancellationToken);

        if (entity == null)
        {
            return new ApiResponse<ShoppingCartResponse>("Record not found!");
        }

       

        ShoppingCartResponse mapped = mapper.Map<ShoppingCartResponse>(entity);
        return new ApiResponse<ShoppingCartResponse>(mapped);
    }


    public async Task<ApiResponse<List<ShoppingCartResponse>>> Handle(GetCompletedShoppingCartsQuery request, CancellationToken cancellationToken)
    {
        List<ShoppingCart> entities = await dbContext.Set<ShoppingCart>()
            .Include(cart => cart.Products)
            .Where(x => x.UserId == request.Id && x.IsCompleted)
            .ToListAsync(cancellationToken);

        if (entities == null || entities.Count == 0)
        {
            return new ApiResponse<List<ShoppingCartResponse>>("Records not found!");
        }

        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.Id);
        var userProfitMargin = user?.ProfitMargin ?? 0;

        if (user != null && user.Role != "admin")
        {
            foreach (var entity in entities)
            {
                foreach (var product in entity.Products)
                {
                    if (product != null)
                    {
                        product.Price = product.Price + product.Price * userProfitMargin / 100;
                        product.Price = product.Price * 0.20m;
                    }
                }
            }
        }

        List<ShoppingCartResponse> mappedList = mapper.Map<List<ShoppingCartResponse>>(entities);
        return new ApiResponse<List<ShoppingCartResponse>>(mappedList);
    }

}
