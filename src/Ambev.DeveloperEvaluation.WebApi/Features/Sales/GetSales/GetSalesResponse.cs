namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

public class GetSalesResponse
{
    public List<SaleResponseDto> Data { get; set; } = new List<SaleResponseDto>();
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}

public class SaleResponseDto
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
    public List<SaleItemResponseDto> Items { get; set; } = new List<SaleItemResponseDto>();
}

public class SaleItemResponseDto
{
    public Guid Id { get; set; }
    public string Product { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }
}
