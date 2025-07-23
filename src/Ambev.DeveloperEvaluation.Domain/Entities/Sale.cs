using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity
{
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public string Customer { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
    public SaleStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public List<SaleItem> Items { get; set; } = new List<SaleItem>();

    public Sale()
    {
        CreatedAt = DateTime.UtcNow;
        Status = SaleStatus.Active;
    }

    public void CalculateTotalAmount()
    {
        TotalAmount = Items.Sum(item => item.TotalAmount);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        Status = SaleStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddItem(SaleItem item)
    {
        ValidateItemQuantity(item);
        Items.Add(item);
        item.CalculateDiscount();
        CalculateTotalAmount();
    }

    public void RemoveItem(Guid itemId)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            Items.Remove(item);
            CalculateTotalAmount();
        }
    }

    private void ValidateItemQuantity(SaleItem item)
    {
        if (item.Quantity > 20)
        {
            throw new ArgumentException("Não é possível vender mais de 20 itens idênticos");
        }
    }
} 