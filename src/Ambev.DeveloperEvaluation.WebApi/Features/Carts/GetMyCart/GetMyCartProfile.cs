using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Carts.GetMyCart;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetMyCart;

public class GetMyCartProfile : Profile
{
    public GetMyCartProfile()
    {
        CreateMap<GetMyCartResult, GetMyCartResponse>();
        CreateMap<GetMyCartProductResult, GetMyCartProductResponseDto>();
    }
}
