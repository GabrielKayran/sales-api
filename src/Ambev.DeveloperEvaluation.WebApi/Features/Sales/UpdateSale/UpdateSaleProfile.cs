using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

public class UpdateSaleProfile : Profile
{
    public UpdateSaleProfile()
    {
        CreateMap<UpdateSaleRequest, UpdateSaleCommand>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        
        CreateMap<UpdateSaleItemDto, UpdateSaleItemRequest>();
        
        CreateMap<UpdateSaleResult, UpdateSaleResponse>();
        
        CreateMap<UpdateSaleItemResult, UpdateSaleItemResponseDto>();
    }
}
