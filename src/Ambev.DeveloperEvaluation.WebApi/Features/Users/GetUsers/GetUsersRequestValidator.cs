using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUsers;

/// <summary>
/// Validator for GetUsersRequest
/// </summary>
public class GetUsersRequestValidator : AbstractValidator<GetUsersRequest>
{
    /// <summary>
    /// Initializes validation rules for GetUsersRequest
    /// </summary>
    public GetUsersRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0");

        RuleFor(x => x.Size)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Size must be between 1 and 100");

        RuleFor(x => x.Order)
            .Must(BeValidOrderFormat)
            .When(x => !string.IsNullOrEmpty(x.Order))
            .WithMessage("Order format is invalid. Use format: 'field asc' or 'field desc'");
    }

    private bool BeValidOrderFormat(string? order)
    {
        if (string.IsNullOrEmpty(order))
            return true;

        var orderParts = order.Split(',');
        var validFields = new[] { "username", "email", "status", "role" };
        var validDirections = new[] { "asc", "desc" };

        foreach (var orderPart in orderParts)
        {
            var trimmedPart = orderPart.Trim();
            var parts = trimmedPart.Split(' ');
            
            if (parts.Length == 0 || parts.Length > 2)
                return false;

            var field = parts[0].ToLower();
            if (!validFields.Contains(field))
                return false;

            if (parts.Length == 2)
            {
                var direction = parts[1].ToLower();
                if (!validDirections.Contains(direction))
                    return false;
            }
        }

        return true;
    }
}
