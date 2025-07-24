using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(10,2)");

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(p => p.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Image)
            .HasMaxLength(500);

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(p => p.UpdatedAt);

        // Configuração do Rating como owned entity
        builder.OwnsOne(p => p.Rating, rating =>
        {
            rating.Property(r => r.Rate)
                .HasColumnType("decimal(3,2)")
                .HasColumnName("Rating_Rate");

            rating.Property(r => r.Count)
                .HasColumnName("Rating_Count");
        });

        // Índices para melhor performance
        builder.HasIndex(p => p.Category);
        builder.HasIndex(p => p.Price);
        builder.HasIndex(p => p.CreatedAt);
    }
}
