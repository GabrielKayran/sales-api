using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

public class GetCartsHandler : IRequestHandler<GetCartsQuery, GetCartsResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    public GetCartsHandler(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    public async Task<GetCartsResult> Handle(GetCartsQuery request, CancellationToken cancellationToken)
    {
        var query = await _cartRepository.GetCartsAsync(cancellationToken);
        
        if (request.UserId.HasValue)
        {
            query = query.Where(c => c.UserId == request.UserId.Value);
        }

        if (!string.IsNullOrEmpty(request.OrderBy))
        {
            query = request.OrderBy.ToLower() switch
            {
                "id" => request.OrderDescending ? query.OrderByDescending(c => c.Id) : query.OrderBy(c => c.Id),
                "userid" => request.OrderDescending ? query.OrderByDescending(c => c.UserId) : query.OrderBy(c => c.UserId),
                "date" => request.OrderDescending ? query.OrderByDescending(c => c.Date) : query.OrderBy(c => c.Date),
                "createdat" => request.OrderDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt),
                _ => query.OrderByDescending(c => c.CreatedAt)
            };
        }
        else
        {
            query = query.OrderByDescending(c => c.CreatedAt);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var carts = await query
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .ToListAsync(cancellationToken);
        
        var totalPages = (int)Math.Ceiling((double)totalCount / request.Size);
        var hasNextPage = request.Page < totalPages;
        var hasPreviousPage = request.Page > 1;

        return new GetCartsResult
        {
            Data = _mapper.Map<List<GetCartsResultDto>>(carts),
            TotalCount = totalCount,
            Page = request.Page,
            TotalPages = totalPages,
            HasNextPage = hasNextPage,
            HasPreviousPage = hasPreviousPage
        };
    }
}
