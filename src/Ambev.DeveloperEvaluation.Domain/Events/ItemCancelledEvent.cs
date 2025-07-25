using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Domain event triggered when a sale item is cancelled
/// </summary>
public class ItemCancelledEvent : INotification
{
    public Guid SaleId { get; }
    public Guid ItemId { get; }
    public string Product { get; }
    public int Quantity { get; }
    public decimal UnitPrice { get; }
    public decimal TotalAmount { get; }
    public DateTime CancelledAt { get; }

    public ItemCancelledEvent(Guid saleId, Guid itemId, string product, int quantity, decimal unitPrice, decimal totalAmount)
    {
        SaleId = saleId;
        ItemId = itemId;
        Product = product;
        Quantity = quantity;
        UnitPrice = unitPrice;
        TotalAmount = totalAmount;
        CancelledAt = DateTime.UtcNow;
    }
} 