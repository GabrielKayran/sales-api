using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetMyCart;

public record GetMyCartQuery(Guid UserId) : IRequest<GetMyCartResult?>;
