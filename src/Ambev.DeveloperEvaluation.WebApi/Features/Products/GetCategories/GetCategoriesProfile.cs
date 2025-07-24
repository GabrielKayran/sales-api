using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.GetCategories;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetCategories;

public class GetCategoriesProfile : Profile
{
    public GetCategoriesProfile()
    {
        CreateMap<GetCategoriesResult, GetCategoriesResponse>();
    }
}
