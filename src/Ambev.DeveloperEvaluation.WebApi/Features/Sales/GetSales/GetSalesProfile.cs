using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

public class GetSalesProfile : Profile
{
    public GetSalesProfile()
    {
        CreateMap<GetSalesRequest, GetSalesQuery>();
        
        CreateMap<GetSalesResult, GetSalesResponse>()
            .ConstructUsing(src => new GetSalesResponse(
                src.Data.Select(s => new SaleResponseDto
                {
                    Id = s.Id,
                    SaleNumber = s.SaleNumber,
                    SaleDate = s.SaleDate,
                    Customer = s.Customer,
                    Branch = s.Branch,
                    Status = s.Status,
                    TotalAmount = s.TotalAmount,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                    Items = s.Items.Select(i => new SaleItemResponseDto
                    {
                        Id = i.Id,
                        Product = i.Product,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        Discount = i.Discount,
                        TotalAmount = i.TotalAmount
                    }).ToList()
                }).ToList(),
                src.TotalItems,
                src.CurrentPage,
                src.Data.Count
            ));
        
        CreateMap<SaleDto, SaleResponseDto>();
        
        CreateMap<SaleItemDto, SaleItemResponseDto>();
    }
}
