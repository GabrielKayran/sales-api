using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetMyCart;

public class GetMyCartProfile : Profile
{
    public GetMyCartProfile()
    {
        CreateMap<Cart, GetMyCartResult>();
        CreateMap<CartProduct, GetMyCartProductResult>();
    }
}
