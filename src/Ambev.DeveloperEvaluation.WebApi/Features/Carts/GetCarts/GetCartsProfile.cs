using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCarts;

public class GetCartsProfile : Profile
{
    public GetCartsProfile()
    {
        CreateMap<GetCartsRequest, GetCartsQuery>();
        CreateMap<GetCartsResult, GetCartsResponse>();
        CreateMap<GetCartsResultDto, GetCartsResponseDto>();
        CreateMap<GetCartsProductResultDto, GetCartsProductResponseDto>();
    }
}
