using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

public class GetCartsQuery : IRequest<GetCartsResult>
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public Guid? UserId { get; set; }
    public string? OrderBy { get; set; }
    public bool OrderDescending { get; set; } = false;
}
