using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart;

public class GetCartQuery : IRequest<GetCartResult>
{
    public Guid Id { get; }
    public Guid CurrentUserId { get; }
    public string CurrentUserRole { get; }

    public GetCartQuery(Guid id, Guid currentUserId, string currentUserRole)
    {
        Id = id;
        CurrentUserId = currentUserId;
        CurrentUserRole = currentUserRole;
    }
}
