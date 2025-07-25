using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Domain event triggered when a sale is modified
/// </summary>
public class SaleModifiedEvent : INotification
{
    public Guid SaleId { get; }
    public string SaleNumber { get; }
    public decimal TotalAmount { get; }
    public decimal PreviousAmount { get; }
    public DateTime ModifiedAt { get; }
    public string ModificationType { get; }

    public SaleModifiedEvent(Guid saleId, string saleNumber, decimal totalAmount, decimal previousAmount, string modificationType)
    {
        SaleId = saleId;
        SaleNumber = saleNumber;
        TotalAmount = totalAmount;
        PreviousAmount = previousAmount;
        ModifiedAt = DateTime.UtcNow;
        ModificationType = modificationType;
    }
} 