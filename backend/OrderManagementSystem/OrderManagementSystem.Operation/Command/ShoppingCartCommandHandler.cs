using AutoMapper;
using OrderManagementSystem.Data.Context;
using OrderManagementSystem.Data.Domain;
using OrderManagementSystem.Schema;
using Microsoft.EntityFrameworkCore;
using static OrderManagementSystem.Operation.Cqrs.ShoppingCartCqrs;
using MediatR;

namespace OrderManagementSystem.Operation.Command;

public class ShoppingCartCommandHandler :
    IRequestHandler<CreateShoppingCartCommand, ApiResponse<ShoppingCartResponse>>,
    IRequestHandler<UpdateShoppingCartByIdCommand, ApiResponse<ShoppingCartResponse>>,
    IRequestHandler<DeleteShoppingCartCommand, ApiResponse>
{
    private readonly OmsDbContext dbContext;
    private readonly IMapper mapper;

    public ShoppingCartCommandHandler(OmsDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<ShoppingCartResponse>> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
    {
        var userId = request.Model.UserId;
        var isDelete = request.Model.isDelete;
        var isMinus = request.Model.isMinus;

        var cartQuery = dbContext.ShoppingCarts
            .Include(cart => cart.Products)
            .Where(cart => cart.UserId == userId && cart.IsCompleted == false);

        var existingCart = await cartQuery.FirstOrDefaultAsync();

        if (existingCart == null)
        {
            var newCart = mapper.Map<ShoppingCart>(request.Model);
            newCart.Products = await GetProductsByIds(request.Model.ProductIds);
            newCart.ProductQuantities = new List<ProductQuantity>();

            foreach (var newProduct in newCart.Products)
            {
                var userProfitMargin = (await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId))?.ProfitMargin ?? 0;

                var productPriceWithMargin = newProduct.Price + (newProduct.Price * userProfitMargin / 100);
                var productPriceWithKDV = productPriceWithMargin + (productPriceWithMargin * 0.20m);

                newCart.ProductQuantities.Add(CreateProductQuantity(newProduct, productPriceWithKDV, 1));
            }

            dbContext.ShoppingCarts.Add(newCart);
        }
        else
        {
            var newProducts = await GetProductsByIds(request.Model.ProductIds);

            foreach (var newProduct in newProducts)
            {
                var existingProductQuantity = existingCart.ProductQuantities
                    .FirstOrDefault(pq => pq.ProductId == newProduct.ProductId);

                var userProfitMargin = (await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId))?.ProfitMargin ?? 0;

                var productPriceWithMargin = newProduct.Price + (newProduct.Price * userProfitMargin / 100);
                var productPriceWithKDV = productPriceWithMargin + (productPriceWithMargin * 0.20m);

                if (existingProductQuantity != null)
                {
                    UpdateExistingProductQuantity(existingProductQuantity, isDelete, isMinus);
                }
                else
                {
                    existingCart.ProductQuantities.Add(CreateProductQuantity(newProduct, productPriceWithKDV, 1));
                }
            }

            existingCart.ProductQuantities.RemoveAll(pq => pq.Quantity == 0);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        var response = mapper.Map<ShoppingCartResponse>(existingCart);
        return new ApiResponse<ShoppingCartResponse>(response);
    }

    private ProductQuantity CreateProductQuantity(Product newProduct, decimal price, int quantity)
    {
        return new ProductQuantity
        {
            ProductId = newProduct.ProductId,
            Name = newProduct.Name,
            Description = newProduct.Description,
            Image = newProduct.Image,
            Price = price,
            Quantity = quantity
        };
    }

    private void UpdateExistingProductQuantity(ProductQuantity existingProductQuantity, bool isDelete, bool isMinus)
    {
        if (isDelete)
        {
            existingProductQuantity.Quantity = 0;
        }
        else
        {
            existingProductQuantity.Quantity += isMinus && existingProductQuantity.Quantity > 0 ? -1 : 1;
        }
    }




    private async Task<List<Product>> GetProductsByIds(List<int> productIds)
    {
        return await dbContext.Products.Where(p => productIds.Contains(p.ProductId)).ToListAsync();
    }

    public async Task<ApiResponse> Handle(DeleteShoppingCartCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Set<ShoppingCart>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
        {
            return new ApiResponse("Record not found!");
        }

        entity.IsCanceled = true;
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
    public async Task<ApiResponse<ShoppingCartResponse>> Handle(UpdateShoppingCartByIdCommand request, CancellationToken cancellationToken)
    {
        var cartId = request.Id;
        var updatedModel = request.Model;

        if (updatedModel.isActive == false)
        {
            var existingCart = await dbContext.ShoppingCarts
            .Include(cart => cart.Products)
            .FirstOrDefaultAsync(cart => cart.Id == cartId);

            if (existingCart == null)
            {
                return new ApiResponse<ShoppingCartResponse>("Shopping cart not found or already completed.");
            }

            existingCart.IsActive = false;
            await dbContext.SaveChangesAsync(cancellationToken);

            var response = mapper.Map<ShoppingCartResponse>(existingCart);
            return new ApiResponse<ShoppingCartResponse>(response);
        }
        else
        {
            var existingCart = await dbContext.ShoppingCarts
            .Include(cart => cart.Products)
            .FirstOrDefaultAsync(cart => cart.Id == cartId && cart.IsCompleted == false);

            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == existingCart.UserId);

            var total = 0;
            if (existingCart == null)
            {
                return new ApiResponse<ShoppingCartResponse>("Shopping cart not found or already completed.");
            }
            foreach (var productQuantity in existingCart.ProductQuantities)
            {
                var existingProduct = await dbContext.Products
                    .FirstOrDefaultAsync(p => p.ProductId == productQuantity.ProductId, cancellationToken);

                if (existingProduct != null)
                {
                    existingProduct.StockQuantity -= productQuantity.Quantity;
                    if (existingProduct.StockQuantity < 0)
                    {
                        return new ApiResponse<ShoppingCartResponse>("We cannot place your order due to insufficient stock.");
                    }
                    total = (int)(total + productQuantity.Quantity * productQuantity.Price);
                }
            }
            existingCart.SelectedAddressId = (int)updatedModel.SelectedAddressId;
            existingCart.SelectedCardId = (int)updatedModel.SelectedCardId;
            existingCart.IsCompleted = (bool)updatedModel.IsCompleted;
            existingCart.WaitForPayment = (bool)updatedModel.WaitForPayment;
            if ((bool)updatedModel.OpenAccount)
            {
                if(user.OpenAccountLimit < total)
                {
                    return new ApiResponse<ShoppingCartResponse>("Open account limit exceeded.");
                }
                else
                {
                    user.OpenAccountLimit -= total;

                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            var response = mapper.Map<ShoppingCartResponse>(existingCart);
            return new ApiResponse<ShoppingCartResponse>(response);
        }
    }
   

}