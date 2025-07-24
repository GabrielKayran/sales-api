using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

public class GetProductsProfile : Profile
{
    public GetProductsProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<ProductRating, ProductRatingDto>();
    }
}
