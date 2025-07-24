using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

public class GetSalesProfile : Profile
{
    public GetSalesProfile()
    {
        CreateMap<Sale, SaleDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        
        CreateMap<SaleItem, SaleItemDto>();
    }
}
