using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;

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

    /// <summary>
    /// Creates a new sale and publishes SaleCreatedEvent
    /// </summary>
    public static Sale Create(string saleNumber, string customer, string branch)
    {
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = saleNumber,
            Customer = customer,
            Branch = branch,
            SaleDate = DateTime.UtcNow
        };

        // Publish SaleCreatedEvent
        sale.AddDomainEvent(new SaleCreatedEvent(
            sale.Id,
            sale.SaleNumber,
            sale.Customer,
            sale.Branch,
            sale.TotalAmount,
            sale.Items.Count
        ));

        return sale;
    }

    public void CalculateTotalAmount()
    {
        var previousAmount = TotalAmount;
        TotalAmount = Items.Sum(item => item.TotalAmount);
        UpdatedAt = DateTime.UtcNow;

        // Publish SaleModifiedEvent if amount changed
        if (previousAmount != TotalAmount && previousAmount > 0)
        {
            AddDomainEvent(new SaleModifiedEvent(
                Id,
                SaleNumber,
                TotalAmount,
                previousAmount,
                "Amount Recalculated"
            ));
        }
    }

    public void Cancel(string reason = "Sale cancelled by user")
    {
        Status = SaleStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;

        // Publish SaleCancelledEvent
        AddDomainEvent(new SaleCancelledEvent(
            Id,
            SaleNumber,
            reason,
            TotalAmount
        ));
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
            // Publish ItemCancelledEvent before removing
            AddDomainEvent(new ItemCancelledEvent(
                Id,
                item.Id,
                item.Product,
                item.Quantity,
                item.UnitPrice,
                item.TotalAmount
            ));

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