using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCarts;

public class GetCartsRequestValidator : AbstractValidator<GetCartsRequest>
{
    public GetCartsRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page deve ser maior que 0");

        RuleFor(x => x.Size)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Size deve estar entre 1 e 100");

        RuleFor(x => x.OrderBy)
            .Must(x => string.IsNullOrEmpty(x) || new[] { "id", "userid", "date", "createdat" }.Contains(x.ToLower()))
            .When(x => !string.IsNullOrEmpty(x.OrderBy))
            .WithMessage("OrderBy deve ser: id, userid, date ou createdat");
    }
}
