namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;

public class GetProductsRequest
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string? Title { get; set; }
    public string? Category { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? OrderBy { get; set; }
    public bool OrderDescending { get; set; } = false;
}
