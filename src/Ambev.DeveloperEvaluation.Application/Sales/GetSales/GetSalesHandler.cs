using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

public class GetSalesHandler : IRequestHandler<GetSalesQuery, GetSalesResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public GetSalesHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<GetSalesResult> Handle(GetSalesQuery request, CancellationToken cancellationToken)
    {
        var query = await _saleRepository.GetSalesAsync();

        // Aplicar filtros
        if (!string.IsNullOrEmpty(request.Customer))
        {
            query = query.Where(s => s.Customer.Contains(request.Customer));
        }

        if (!string.IsNullOrEmpty(request.Branch))
        {
            query = query.Where(s => s.Branch.Contains(request.Branch));
        }

        if (request.MinDate.HasValue)
        {
            query = query.Where(s => s.SaleDate >= request.MinDate.Value);
        }

        if (request.MaxDate.HasValue)
        {
            query = query.Where(s => s.SaleDate <= request.MaxDate.Value);
        }

        if (!string.IsNullOrEmpty(request.Status))
        {
            if (Enum.TryParse<Domain.Enums.SaleStatus>(request.Status, true, out var status))
            {
                query = query.Where(s => s.Status == status);
            }
        }

        if (!string.IsNullOrEmpty(request.Order))
        {
            query = ApplyOrdering(query, request.Order);
        }
        else
        {
            query = query.OrderByDescending(s => s.CreatedAt);
        }

        var totalItems = query.Count();

        var sales = query
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .ToList();

        var totalPages = (int)Math.Ceiling((double)totalItems / request.Size);

        return new GetSalesResult
        {
            Data = _mapper.Map<List<SaleDto>>(sales),
            TotalItems = totalItems,
            CurrentPage = request.Page,
            TotalPages = totalPages
        };
    }

    private static IQueryable<Sale> ApplyOrdering(IQueryable<Sale> query, string order)
    {
        var orderParts = order.Split(',');
        
        foreach (var part in orderParts)
        {
            var orderPart = part.Trim();
            var isDescending = orderPart.EndsWith(" desc", StringComparison.OrdinalIgnoreCase);
            var fieldName = isDescending ? orderPart[..^5].Trim() : orderPart;

            query = fieldName.ToLower() switch
            {
                "salenumber" => isDescending ? query.OrderByDescending(s => s.SaleNumber) : query.OrderBy(s => s.SaleNumber),
                "saledate" => isDescending ? query.OrderByDescending(s => s.SaleDate) : query.OrderBy(s => s.SaleDate),
                "customer" => isDescending ? query.OrderByDescending(s => s.Customer) : query.OrderBy(s => s.Customer),
                "branch" => isDescending ? query.OrderByDescending(s => s.Branch) : query.OrderBy(s => s.Branch),
                "totalamount" => isDescending ? query.OrderByDescending(s => s.TotalAmount) : query.OrderBy(s => s.TotalAmount),
                "createdat" => isDescending ? query.OrderByDescending(s => s.CreatedAt) : query.OrderBy(s => s.CreatedAt),
                _ => query
            };
        }

        return query;
    }
}
