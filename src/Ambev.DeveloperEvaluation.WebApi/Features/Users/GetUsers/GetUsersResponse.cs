using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUsers;

/// <summary>
/// Response model for getting paginated users
/// </summary>
public class GetUsersResponse : PaginatedList<GetUsersUserDto>
{
    public GetUsersResponse(List<GetUsersUserDto> items, int count, int pageNumber, int pageSize) 
        : base(items, count, pageNumber, pageSize)
    {
    }
}

/// <summary>
/// DTO for user data in GetUsers response
/// </summary>
public class GetUsersUserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public GetUsersUserNameDto Name { get; set; } = new();
    public GetUsersUserAddressDto Address { get; set; } = new();
    public string Phone { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

/// <summary>
/// DTO for user name
/// </summary>
public class GetUsersUserNameDto
{
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
}

/// <summary>
/// DTO for user address
/// </summary>
public class GetUsersUserAddressDto
{
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public int Number { get; set; }
    public string Zipcode { get; set; } = string.Empty;
    public GetUsersUserGeolocationDto Geolocation { get; set; } = new();
}

/// <summary>
/// DTO for user geolocation
/// </summary>
public class GetUsersUserGeolocationDto
{
    public string Lat { get; set; } = string.Empty;
    public string Long { get; set; } = string.Empty;
}
