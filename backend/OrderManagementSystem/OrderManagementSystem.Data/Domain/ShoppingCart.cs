using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementSystem.Data.Domain;

[Table("ShoppingCart", Schema = "dbo")]
public class ShoppingCart : BaseModel
{
    public int UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsCompleted { get; set; }
    public int SelectedAddressId { get; set; }
    public int SelectedCardId { get; set; }
    public bool IsCanceled { get; set; }
    public bool WaitForPayment { get; set; }
    public virtual List<Product> Products { get; set; }
    public virtual List<ProductQuantity> ProductQuantities { get; set; }
}
public class ProductQuantity
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
{
    public void Configure(EntityTypeBuilder<ShoppingCart> builder)
    {
        builder.Property(x => x.InsertUserId).IsRequired();
        builder.Property(x => x.UpdateUserId).IsRequired().HasDefaultValue(0);
        builder.Property(x => x.InsertDate).IsRequired();
        builder.Property(x => x.UpdateDate).IsRequired(false);
        builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.TotalPrice).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.IsCompleted).IsRequired().HasDefaultValue(false);
        builder.Property(x => x.IsCanceled).IsRequired().HasDefaultValue(false);
        builder.Property(x => x.WaitForPayment).IsRequired().HasDefaultValue(false);
        builder.Property(x => x.SelectedAddressId).IsRequired().HasDefaultValue(0);
        builder.Property(x => x.SelectedCardId).IsRequired().HasDefaultValue(0);


        builder.HasMany(sc => sc.Products)
            .WithMany(p => p.ShoppingCarts)
            .UsingEntity(j => j.ToTable("ShoppingCartProduct"));

        builder.OwnsMany(x => x.ProductQuantities, pq =>
        {
            pq.Property(p => p.ProductId);
            pq.Property(p => p.Quantity);
        });
    }
}