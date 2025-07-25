using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Domain event triggered when a sale is created
/// </summary>
public class SaleCreatedEvent : INotification
{
    public Guid SaleId { get; }
    public string SaleNumber { get; }
    public string Customer { get; }
    public string Branch { get; }
    public decimal TotalAmount { get; }
    public DateTime CreatedAt { get; }
    public int ItemCount { get; }

    public SaleCreatedEvent(Guid saleId, string saleNumber, string customer, string branch, decimal totalAmount, int itemCount)
    {
        SaleId = saleId;
        SaleNumber = saleNumber;
        Customer = customer;
        Branch = branch;
        TotalAmount = totalAmount;
        CreatedAt = DateTime.UtcNow;
        ItemCount = itemCount;
    }
} 