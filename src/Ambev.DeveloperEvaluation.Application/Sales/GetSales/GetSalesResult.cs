namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

public class GetSalesResult
{
    public List<SaleDto> Data { get; set; } = new List<SaleDto>();
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}

public class SaleDto
{
    public Guid Id { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public string Customer { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<SaleItemDto> Items { get; set; } = new List<SaleItemDto>();
}

public class SaleItemDto
{
    public Guid Id { get; set; }
    public string Product { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }
}
