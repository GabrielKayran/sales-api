using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetMyCart;

public class GetMyCartHandler : IRequestHandler<GetMyCartQuery, GetMyCartResult?>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    public GetMyCartHandler(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    public async Task<GetMyCartResult?> Handle(GetMyCartQuery request, CancellationToken cancellationToken)
    {
        var carts = await _cartRepository.GetCartsByUserIdAsync(request.UserId, cancellationToken);
        
        // Retorna o carrinho mais recente do usuário (assumindo que só deve ter um ativo)
        var activeCart = carts.OrderByDescending(c => c.CreatedAt).FirstOrDefault();
        
        if (activeCart == null)
            return null;

        return _mapper.Map<GetMyCartResult>(activeCart);
    }
}
