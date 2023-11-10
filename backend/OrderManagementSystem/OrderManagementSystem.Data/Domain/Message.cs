using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using OrderManagementSystem.Base;

namespace OrderManagementSystem.Data.Domain;

[Table("Message", Schema = "dbo")]
public class Message : BaseModel
{
    public int ChatId { get; set; }
    public string Content { get; set; }
    public string Email { get; set; }
    public bool IsAdmin { get; set; }
}


public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.Property(x => x.InsertUserId).IsRequired();
        builder.Property(x => x.UpdateUserId).IsRequired().HasDefaultValue(0);
        builder.Property(x => x.InsertDate).IsRequired();
        builder.Property(x => x.UpdateDate).IsRequired(false);
        builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);

        builder.Property(x => x.ChatId).IsRequired();
        builder.Property(x => x.IsAdmin).IsRequired();

    }
}