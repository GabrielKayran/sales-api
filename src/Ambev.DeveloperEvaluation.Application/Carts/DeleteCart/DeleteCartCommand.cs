using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

public class DeleteCartCommand : IRequest<bool>
{
    public Guid Id { get; }
    public Guid CurrentUserId { get; }
    public string CurrentUserRole { get; }

    public DeleteCartCommand(Guid id, Guid currentUserId, string currentUserRole)
    {
        Id = id;
        CurrentUserId = currentUserId;
        CurrentUserRole = currentUserRole;
    }
}
