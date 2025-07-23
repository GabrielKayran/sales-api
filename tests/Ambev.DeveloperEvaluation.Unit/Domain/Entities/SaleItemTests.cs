using Ambev.DeveloperEvaluation.Domain.Entities;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleItemTests
{
    [Theory]
    [InlineData(1, 100, 0)] // Menos de 4 itens - sem desconto
    [InlineData(3, 100, 0)] // Menos de 4 itens - sem desconto
    [InlineData(4, 100, 40)] // 4 itens - 10% desconto
    [InlineData(5, 100, 50)] // 5 itens - 10% desconto
    [InlineData(9, 100, 90)] // 9 itens - 10% desconto
    [InlineData(10, 100, 200)] // 10 itens - 20% desconto
    [InlineData(15, 100, 300)] // 15 itens - 20% desconto
    [InlineData(20, 100, 400)] // 20 itens - 20% desconto
    public void CalculateDiscount_ShouldApplyCorrectDiscountPercentage(int quantity, decimal unitPrice, decimal expectedDiscount)
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Product = "Produto Teste",
            Quantity = quantity,
            UnitPrice = unitPrice
        };

        // Act
        saleItem.CalculateDiscount();

        // Assert
        Assert.Equal(expectedDiscount, saleItem.Discount);
        var expectedTotal = (quantity * unitPrice) - expectedDiscount;
        Assert.Equal(expectedTotal, saleItem.TotalAmount);
    }

    [Fact]
    public void CalculateDiscount_WithQuantityAbove20_ShouldThrowException()
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Product = "Produto Teste",
            Quantity = 21,
            UnitPrice = 100m
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => saleItem.CalculateDiscount());
    }

    [Fact]
    public void CalculateDiscount_WithZeroQuantity_ShouldThrowException()
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Product = "Produto Teste",
            Quantity = 0,
            UnitPrice = 100m
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => saleItem.CalculateDiscount());
    }

    [Fact]
    public void UpdateQuantity_ShouldRecalculateDiscount()
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Product = "Produto Teste",
            Quantity = 3,
            UnitPrice = 100m
        };
        saleItem.CalculateDiscount();
        
        Assert.Equal(0, saleItem.Discount); // Inicialmente sem desconto

        // Act
        saleItem.UpdateQuantity(5);

        // Assert
        Assert.Equal(5, saleItem.Quantity);
        Assert.Equal(50m, saleItem.Discount); // Agora com 10% de desconto
        Assert.Equal(450m, saleItem.TotalAmount);
    }
} 