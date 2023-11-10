using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Base;
using System.ComponentModel.DataAnnotations.Schema;


namespace OrderManagementSystem.Data.Domain;

[Table("Product", Schema = "dbo")]
public class Product : BaseModel
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Image { get; set; }
    public decimal ProfitMargin { get; set; }
    public virtual List<ShoppingCart> ShoppingCarts { get; set; }
}

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.InsertUserId).IsRequired();
        builder.Property(x => x.UpdateUserId).HasDefaultValue(0);
        builder.Property(x => x.InsertDate).IsRequired();
        builder.Property(x => x.UpdateDate).IsRequired(false);
        builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);

        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(70);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Price).IsRequired();
        builder.Property(x => x.StockQuantity).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.Image).IsRequired().HasMaxLength(255);

        builder.HasIndex(x => x.ProductId).IsUnique(true);
       

        builder.HasMany(p => p.ShoppingCarts)
            .WithMany(sc => sc.Products)
            .UsingEntity(j => j.ToTable("ShoppingCartProduct"));
    }
}