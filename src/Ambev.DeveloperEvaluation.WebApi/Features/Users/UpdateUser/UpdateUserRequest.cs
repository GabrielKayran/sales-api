namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateUser;

/// <summary>
/// Request model for updating a user
/// </summary>
public class UpdateUserRequest
{
    /// <summary>
    /// The username of the user
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// The password for the user (optional for updates)
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// The phone number of the user
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// The email address of the user
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The status of the user (Active, Inactive, Suspended)
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// The role of the user (Customer, Manager, Admin)
    /// </summary>
    public string Role { get; set; } = string.Empty;
}
