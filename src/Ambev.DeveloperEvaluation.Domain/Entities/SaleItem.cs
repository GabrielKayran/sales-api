using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem : BaseEntity
{
    public Guid SaleId { get; set; }
    public string Product { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }
    
    public Sale Sale { get; set; }

    public void CalculateDiscount()
    {
        ValidateQuantity();
        
        decimal discountPercentage = GetDiscountPercentage();
        Discount = (UnitPrice * Quantity) * discountPercentage;
        TotalAmount = (UnitPrice * Quantity) - Discount;
    }

    private decimal GetDiscountPercentage()
    {
        if (Quantity < 4)
        {
            return 0m;
        }
        else if (Quantity >= 4 && Quantity < 10)
        {
            return 0.10m; // 10%
        }
        else if (Quantity >= 10 && Quantity <= 20)
        {
            return 0.20m; // 20%
        }
        
        throw new ArgumentException("Quantidade inválida para cálculo de desconto");
    }

    private void ValidateQuantity()
    {
        if (Quantity <= 0)
        {
            throw new ArgumentException("Quantidade deve ser maior que zero");
        }
        
        if (Quantity > 20)
        {
            throw new ArgumentException("Não é possível vender mais de 20 itens idênticos");
        }
    }

    public void UpdateQuantity(int newQuantity)
    {
        Quantity = newQuantity;
        CalculateDiscount();
    }
} 