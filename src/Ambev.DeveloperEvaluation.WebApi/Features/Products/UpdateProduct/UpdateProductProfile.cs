using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

public class UpdateProductProfile : Profile
{
    public UpdateProductProfile()
    {
        CreateMap<UpdateProductRequest, UpdateProductCommand>();
        CreateMap<UpdateProductRatingDto, UpdateProductRatingRequest>();
        CreateMap<UpdateProductResult, UpdateProductResponse>();
        CreateMap<UpdateProductRatingResult, UpdateProductRatingResponseDto>();
    }
}
