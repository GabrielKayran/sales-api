using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleTests
{
    [Fact]
    public void Sale_Creation_ShouldSetDefaultValues()
    {
        // Arrange & Act
        var sale = new Sale();

        // Assert
        Assert.Equal(SaleStatus.Active, sale.Status);
        Assert.True(sale.CreatedAt <= DateTime.UtcNow);
        Assert.Empty(sale.Items);
    }

    [Fact]
    public void AddItem_ShouldCalculateDiscountCorrectly()
    {
        // Arrange
        var sale = new Sale
        {
            SaleNumber = "SALE001",
            Customer = "Cliente Teste",
            Branch = "Filial 01",
            SaleDate = DateTime.UtcNow.Date
        };

        var item = new SaleItem
        {
            Product = "Produto A",
            Quantity = 5, // Deve ter 10% de desconto
            UnitPrice = 100m
        };

        // Act
        sale.AddItem(item);

        // Assert
        Assert.Single(sale.Items);
        Assert.Equal(50m, item.Discount); // 5 * 100 * 0.10 = 50
        Assert.Equal(450m, item.TotalAmount); // 500 - 50 = 450
        Assert.Equal(450m, sale.TotalAmount);
    }

    [Fact]
    public void AddItem_WithQuantityAbove20_ShouldThrowException()
    {
        // Arrange
        var sale = new Sale();
        var item = new SaleItem
        {
            Product = "Produto A",
            Quantity = 21,
            UnitPrice = 100m
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => sale.AddItem(item));
    }

    [Fact]
    public void Cancel_ShouldUpdateStatusAndDate()
    {
        // Arrange
        var sale = new Sale();

        // Act
        sale.Cancel();

        // Assert
        Assert.Equal(SaleStatus.Cancelled, sale.Status);
        Assert.NotNull(sale.UpdatedAt);
        Assert.True(sale.UpdatedAt <= DateTime.UtcNow);
    }
} 