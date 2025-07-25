namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateUser;

/// <summary>
/// Response model for updating a user
/// </summary>
public class UpdateUserResponse
{
    /// <summary>
    /// The unique identifier of the user
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The username of the user
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// The email address of the user
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The phone number of the user
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// The status of the user
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// The role of the user
    /// </summary>
    public string Role { get; set; } = string.Empty;
}
