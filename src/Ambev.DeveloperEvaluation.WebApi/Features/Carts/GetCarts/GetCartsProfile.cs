using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCarts;

public class GetCartsProfile : Profile
{
    public GetCartsProfile()
    {
        CreateMap<GetCartsRequest, GetCartsQuery>();
        
        CreateMap<GetCartsResult, GetCartsResponse>()
            .ConstructUsing(src => new GetCartsResponse(
                src.Data.Select(c => new GetCartsResponseDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    Date = c.Date,
                    Products = c.Products.Select(p => new GetCartsProductResponseDto
                    {
                        ProductId = p.ProductId,
                        Quantity = p.Quantity
                    }).ToList(),
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                }).ToList(),
                src.TotalCount,
                src.Page,
                src.Data.Count
            ));
        
        CreateMap<GetCartsResultDto, GetCartsResponseDto>();
        CreateMap<GetCartsProductResultDto, GetCartsProductResponseDto>();
    }
}
