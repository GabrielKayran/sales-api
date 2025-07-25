using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;

public class UpdateCartProfile : Profile
{
    public UpdateCartProfile()
    {
        CreateMap<UpdateCartRequest, UpdateCartCommand>();
        CreateMap<UpdateCartResult, UpdateCartResponse>();
        CreateMap<UpdateCartProductDto, UpdateCartProductCommand>();
        CreateMap<UpdateCartProductResult, UpdateCartProductResponseDto>();
    }
}
