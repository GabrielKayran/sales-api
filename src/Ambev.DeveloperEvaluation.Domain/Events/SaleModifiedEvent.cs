namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleModifiedEvent
{
    public Guid SaleId { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime ModifiedAt { get; set; }
    public string ModificationType { get; set; } = string.Empty;
} 