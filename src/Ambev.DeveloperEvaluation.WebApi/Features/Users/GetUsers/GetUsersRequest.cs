namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUsers;

/// <summary>
/// Request model for getting paginated users
/// </summary>
public class GetUsersRequest
{
    /// <summary>
    /// Page number for pagination (default: 1)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Number of items per page (default: 10)
    /// </summary>
    public int Size { get; set; } = 10;

    /// <summary>
    /// Ordering criteria (e.g., "username asc, email desc")
    /// </summary>
    public string? Order { get; set; }
}
