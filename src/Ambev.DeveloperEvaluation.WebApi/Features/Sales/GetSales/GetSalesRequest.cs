using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

public class GetSalesRequest
{
    [FromQuery(Name = "_page")]
    public int Page { get; set; } = 1;

    [FromQuery(Name = "_size")]
    public int Size { get; set; } = 10;

    [FromQuery(Name = "_order")]
    public string? Order { get; set; }

    [FromQuery(Name = "customer")]
    public string? Customer { get; set; }

    [FromQuery(Name = "branch")]
    public string? Branch { get; set; }

    [FromQuery(Name = "_minDate")]
    public DateTime? MinDate { get; set; }

    [FromQuery(Name = "_maxDate")]
    public DateTime? MaxDate { get; set; }

    [FromQuery(Name = "status")]
    public string? Status { get; set; }
}
