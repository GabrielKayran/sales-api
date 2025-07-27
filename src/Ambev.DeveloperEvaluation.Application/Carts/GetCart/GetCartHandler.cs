using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart;

public class GetCartHandler : IRequestHandler<GetCartQuery, GetCartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    public GetCartHandler(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    public async Task<GetCartResult> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByIdAsync(request.Id, cancellationToken);
        if (cart == null)
            throw new KeyNotFoundException($"Carrinho com ID {request.Id} não foi encontrado");

        var isAdmin = request.CurrentUserRole == nameof(UserRole.Admin);
        var isOwner = cart.UserId == request.CurrentUserId;
        
        if (!isAdmin && !isOwner)
        {
            throw new UnauthorizedAccessException("Você só pode visualizar seus próprios carrinhos");
        }

        return _mapper.Map<GetCartResult>(cart);
    }
}
