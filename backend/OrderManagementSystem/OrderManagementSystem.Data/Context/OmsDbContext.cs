using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Data.Domain;

namespace OrderManagementSystem.Data.Context;

public class OmsDbContext: DbContext
{
    public OmsDbContext(DbContextOptions<OmsDbContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<Card> Card { get; set; }
    public DbSet<Address> Adress { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new ShoppingCartConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new MessageConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}

