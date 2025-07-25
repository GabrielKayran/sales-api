using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

public class CreateProductProfile : Profile
{
    public CreateProductProfile()
    {
        // Map Request to Command
        CreateMap<CreateProductRequest, CreateProductCommand>();
        
        // Map Rating DTO to Rating Request
        CreateMap<CreateProductRatingDto, CreateProductRatingRequest>();
        
        // Map Rating Result to Rating Response DTO
        CreateMap<CreateProductRatingResult, CreateProductRatingResponseDto>();
        
        // Map Result to Response
        CreateMap<CreateProductResult, CreateProductResponse>();
    }
}
