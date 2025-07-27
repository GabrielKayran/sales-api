using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

public class DeleteCartHandler : IRequestHandler<DeleteCartCommand, bool>
{
    private readonly ICartRepository _cartRepository;

    public DeleteCartHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<bool> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByIdAsync(request.Id, cancellationToken);
        if (cart == null)
            throw new KeyNotFoundException($"Carrinho com ID {request.Id} não foi encontrado");

        // Verificar se o usuário é o proprietário do carrinho ou Admin
        var isAdmin = request.CurrentUserRole == nameof(UserRole.Admin);
        var isOwner = cart.UserId == request.CurrentUserId;
        
        if (!isAdmin && !isOwner)
        {
            throw new UnauthorizedAccessException("Você só pode excluir seus próprios carrinhos");
        }

        return await _cartRepository.DeleteAsync(request.Id, cancellationToken);
    }
}
