using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateUser;

/// <summary>
/// Validator for UpdateUserRequest
/// </summary>
public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    /// <summary>
    /// Initializes validation rules for UpdateUserRequest
    /// </summary>
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required")
            .Length(3, 50)
            .WithMessage("Username must be between 3 and 50 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email format");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Phone is required")
            .Matches(@"^\+?[\d\s\-\(\)]+$")
            .WithMessage("Invalid phone format");

        RuleFor(x => x.Password)
            .MinimumLength(8)
            .When(x => !string.IsNullOrEmpty(x.Password))
            .WithMessage("Password must be at least 8 characters long");

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required")
            .Must(BeValidStatus)
            .WithMessage("Status must be one of: Active, Inactive, Suspended");

        RuleFor(x => x.Role)
            .NotEmpty()
            .WithMessage("Role is required")
            .Must(BeValidRole)
            .WithMessage("Role must be one of: Customer, Manager, Admin");
    }

    private bool BeValidStatus(string status)
    {
        var validStatuses = new[] { "Active", "Inactive", "Suspended" };
        return validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase);
    }

    private bool BeValidRole(string role)
    {
        var validRoles = new[] { "Customer", "Manager", "Admin" };
        return validRoles.Contains(role, StringComparer.OrdinalIgnoreCase);
    }
}
