using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

public class GetCartsProfile : Profile
{
    public GetCartsProfile()
    {
        CreateMap<Cart, GetCartsResultDto>();
        CreateMap<CartProduct, GetCartsProductResultDto>();
    }
}
