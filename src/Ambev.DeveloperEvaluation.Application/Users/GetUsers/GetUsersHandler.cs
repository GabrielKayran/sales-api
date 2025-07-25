using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUsers;

/// <summary>
/// Handler for processing GetUsersQuery requests
/// </summary>
public class GetUsersHandler : IRequestHandler<GetUsersQuery, GetUsersResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetUsersHandler
    /// </summary>
    /// <param name="userRepository">The user repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetUsersHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetUsersQuery request
    /// </summary>
    /// <param name="request">The GetUsers query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The paginated users result</returns>
    public async Task<GetUsersResult> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var usersQuery = await _userRepository.GetUsersAsync(cancellationToken);

        // Apply ordering if specified
        if (!string.IsNullOrEmpty(request.Order))
        {
            usersQuery = ApplyOrdering(usersQuery, request.Order);
        }
        else
        {
            usersQuery = usersQuery.OrderBy(u => u.Username);
        }

        // Get total count before pagination
        var totalItems = await usersQuery.CountAsync(cancellationToken);

        // Apply pagination
        var users = await usersQuery
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalItems / request.Size);

        return new GetUsersResult
        {
            Data = _mapper.Map<List<UserDto>>(users),
            TotalItems = totalItems,
            CurrentPage = request.Page,
            TotalPages = totalPages
        };
    }

    private IQueryable<Domain.Entities.User> ApplyOrdering(IQueryable<Domain.Entities.User> query, string order)
    {
        var orderParts = order.Split(',');
        
        foreach (var orderPart in orderParts)
        {
            var trimmedPart = orderPart.Trim();
            var parts = trimmedPart.Split(' ');
            var field = parts[0].ToLower();
            var direction = parts.Length > 1 ? parts[1].ToLower() : "asc";

            query = field switch
            {
                "username" => direction == "desc" ? query.OrderByDescending(u => u.Username) : query.OrderBy(u => u.Username),
                "email" => direction == "desc" ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                "status" => direction == "desc" ? query.OrderByDescending(u => u.Status) : query.OrderBy(u => u.Status),
                "role" => direction == "desc" ? query.OrderByDescending(u => u.Role) : query.OrderBy(u => u.Role),
                _ => query.OrderBy(u => u.Username)
            };
        }

        return query;
    }
}
