using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.GetProducts;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class GetProductsProfile : Profile
{
    public GetProductsProfile()
    {
        CreateMap<GetProductsRequest, GetProductsQuery>();
            
        CreateMap<ProductDto, GetProductsResponseDto>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt ?? src.CreatedAt));
            
        CreateMap<ProductRatingDto, GetProductsRatingResponseDto>();
    } 
}
