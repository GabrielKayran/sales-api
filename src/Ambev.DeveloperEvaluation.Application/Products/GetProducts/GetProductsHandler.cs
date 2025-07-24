using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

public class GetProductsHandler : IRequestHandler<GetProductsQuery, GetProductsResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductsHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<GetProductsResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var query = await _productRepository.GetProductsAsync(cancellationToken);

        // Aplicar filtros
        if (!string.IsNullOrEmpty(request.Title))
        {
            query = query.Where(p => p.Title.Contains(request.Title));
        }

        if (!string.IsNullOrEmpty(request.Category))
        {
            query = query.Where(p => p.Category.Contains(request.Category));
        }

        if (request.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= request.MinPrice.Value);
        }

        if (request.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= request.MaxPrice.Value);
        }

        // Aplicar ordenação
        if (!string.IsNullOrEmpty(request.Order))
        {
            query = ApplyOrdering(query, request.Order);
        }
        else
        {
            query = query.OrderByDescending(p => p.CreatedAt);
        }

        // Contar total de itens
        var totalItems = query.Count();

        // Aplicar paginação
        var products = query
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .ToList();

        var totalPages = (int)Math.Ceiling((double)totalItems / request.Size);

        return new GetProductsResult
        {
            Data = _mapper.Map<List<ProductDto>>(products),
            TotalItems = totalItems,
            CurrentPage = request.Page,
            TotalPages = totalPages
        };
    }

    private static IQueryable<Product> ApplyOrdering(IQueryable<Product> query, string order)
    {
        var orderParts = order.Split(',');
        
        foreach (var part in orderParts)
        {
            var orderPart = part.Trim();
            var isDescending = orderPart.EndsWith(" desc", StringComparison.OrdinalIgnoreCase);
            var fieldName = isDescending ? orderPart[..^5].Trim() : orderPart;

            query = fieldName.ToLower() switch
            {
                "title" => isDescending ? query.OrderByDescending(p => p.Title) : query.OrderBy(p => p.Title),
                "price" => isDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                "category" => isDescending ? query.OrderByDescending(p => p.Category) : query.OrderBy(p => p.Category),
                "createdat" => isDescending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
                _ => query
            };
        }

        return query;
    }
}
