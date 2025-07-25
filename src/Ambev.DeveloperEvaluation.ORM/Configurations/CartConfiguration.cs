using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.ORM.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(c => c.UserId)
            .IsRequired()
            .HasColumnType("uuid");

        builder.Property(c => c.Date)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.Property(c => c.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.Property(c => c.UpdatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone");
        
        builder.OwnsMany(c => c.Products, cp =>
        {
            cp.ToTable("CartProducts");
            
            cp.WithOwner().HasForeignKey("CartId");
            
            cp.Property<Guid>("CartId")
                .HasColumnType("uuid");

            cp.Property(p => p.ProductId)
                .IsRequired()
                .HasColumnType("uuid");

            cp.Property(p => p.Quantity)
                .IsRequired()
                .HasColumnType("integer");

            cp.HasKey("CartId", "ProductId");
        });
        
        builder.HasIndex(c => c.UserId)
            .HasDatabaseName("IX_Carts_UserId");

        builder.HasIndex(c => c.Date)
            .HasDatabaseName("IX_Carts_Date");

        builder.HasIndex(c => c.CreatedAt)
            .HasDatabaseName("IX_Carts_CreatedAt");
    }
}
