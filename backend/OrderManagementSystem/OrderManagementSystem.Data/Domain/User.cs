using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.Data.Domain;

[Table("User", Schema = "dbo")]
public class User : BaseModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; }
    public DateTime LastActivityDate { get; set; }
    public int PasswordRetryCount { get; set; }
    public int Status { get; set; }
    public decimal ProfitMargin { get; set; }
    public int OpenAccountLimit { get; set; }
    public virtual List<Address>? Addresses { get; set; }
    public virtual List<Card>? Cards { get; set; }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.InsertUserId).IsRequired();
        builder.Property(x => x.UpdateUserId).IsRequired().HasDefaultValue(0);
        builder.Property(x => x.InsertDate).IsRequired().HasDefaultValueSql("GETDATE()");
        builder.Property(x => x.UpdateDate).IsRequired(false);
        builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Email).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Password).IsRequired().HasMaxLength(50);
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Role).IsRequired().HasMaxLength(10);
        builder.Property(x => x.LastActivityDate).IsRequired();
        builder.Property(x => x.PasswordRetryCount).IsRequired().HasDefaultValue(0);
        builder.Property(x => x.ProfitMargin).IsRequired().HasDefaultValue(10);
        builder.Property(x => x.OpenAccountLimit).IsRequired().HasDefaultValue(1000);
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.UserId).IsRequired().UseIdentityColumn();
        builder.HasIndex(x => x.UserId).IsUnique(true);
        builder.HasIndex(x => x.Email).IsUnique(true);

        builder.HasMany(x => x.Addresses)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .IsRequired(true);

        builder.HasMany(x => x.Cards)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .IsRequired(true);

    }
}