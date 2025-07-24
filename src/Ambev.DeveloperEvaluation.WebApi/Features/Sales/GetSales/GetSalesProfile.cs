using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

public class GetSalesProfile : Profile
{
    public GetSalesProfile()
    {
        CreateMap<GetSalesRequest, GetSalesQuery>();
        
        CreateMap<GetSalesResult, GetSalesResponse>();
        
        CreateMap<SaleDto, SaleResponseDto>();
        
        CreateMap<SaleItemDto, SaleItemResponseDto>();
    }
}
