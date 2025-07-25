using MediatR;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Domain event triggered when a user is registered
/// </summary>
public class UserRegisteredEvent : INotification
{
    public Guid UserId { get; }
    public string Username { get; }
    public string Email { get; }
    public DateTime RegisteredAt { get; }

    public UserRegisteredEvent(User user)
    {
        UserId = user.Id;
        Username = user.Username;
        Email = user.Email;
        RegisteredAt = DateTime.UtcNow;
    }
}
