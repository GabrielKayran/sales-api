using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem : BaseEntity
{
    public Guid SaleId { get; init; }
    public string Product { get; init; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; init; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }
    
    public Sale? Sale { get; init; }

    public void CalculateDiscount()
    {
        ValidateQuantity();
        
        var discountPercentage = GetDiscountPercentage();
        Discount = (UnitPrice * Quantity) * discountPercentage;
        TotalAmount = (UnitPrice * Quantity) - Discount;
    }

    private decimal GetDiscountPercentage()
    {
        return Quantity switch
        {
            < 4 => 0m,
            >= 4 and < 10 => 0.10m,
            >= 10 and <= 20 => 0.20m,
            _ => throw new ArgumentException("Quantidade inválida para cálculo de desconto")
        };
    }

    private void ValidateQuantity()
    {
        switch (Quantity)
        {
            case <= 0:
                throw new ArgumentException("Quantidade deve ser maior que zero");
            case > 20:
                throw new ArgumentException("Não é possível vender mais de 20 itens idênticos");
        }
    }

    public void UpdateQuantity(int newQuantity)
    {
        Quantity = newQuantity;
        CalculateDiscount();
    }
} 