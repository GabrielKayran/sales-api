using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

public class GetSalesRequestValidator : AbstractValidator<GetSalesRequest>
{
    public GetSalesRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0");

        RuleFor(x => x.Size)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Size must be between 1 and 100");

        RuleFor(x => x.MinDate)
            .LessThanOrEqualTo(x => x.MaxDate)
            .When(x => x.MinDate.HasValue && x.MaxDate.HasValue)
            .WithMessage("MinDate must be less than or equal to MaxDate");

        RuleFor(x => x.Status)
            .Must(BeValidStatus)
            .When(x => !string.IsNullOrEmpty(x.Status))
            .WithMessage("Status must be 'Active' or 'Cancelled'");
    }

    private static bool BeValidStatus(string? status)
    {
        if (string.IsNullOrEmpty(status))
            return true;

        return status.Equals("Active", StringComparison.OrdinalIgnoreCase) ||
               status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase);
    }
}
