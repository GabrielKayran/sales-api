using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCarts;

public class GetCartsResponse : PaginatedList<GetCartsResponseDto>
{
    public GetCartsResponse(List<GetCartsResponseDto> items, int count, int pageNumber, int pageSize) 
        : base(items, count, pageNumber, pageSize)
    {
    }
}

public class GetCartsResponseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public List<GetCartsProductResponseDto> Products { get; set; } = new List<GetCartsProductResponseDto>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class GetCartsProductResponseDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
