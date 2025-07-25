using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

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

        return await _cartRepository.DeleteAsync(request.Id, cancellationToken);
    }
}
