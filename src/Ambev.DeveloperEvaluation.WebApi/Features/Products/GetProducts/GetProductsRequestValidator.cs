using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;

public class GetProductsRequestValidator : AbstractValidator<GetProductsRequest>
{
    public GetProductsRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page deve ser maior que 0");

        RuleFor(x => x.Size)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Size deve estar entre 1 e 100");

        RuleFor(x => x.Title)
            .MaximumLength(200)
            .When(x => !string.IsNullOrEmpty(x.Title))
            .WithMessage("Title deve ter no máximo 200 caracteres");

        RuleFor(x => x.Category)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.Category))
            .WithMessage("Category deve ter no máximo 50 caracteres");

        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MinPrice.HasValue)
            .WithMessage("MinPrice deve ser maior ou igual a 0");

        RuleFor(x => x.MaxPrice)
            .GreaterThan(0)
            .When(x => x.MaxPrice.HasValue)
            .WithMessage("MaxPrice deve ser maior que 0");

        RuleFor(x => x.OrderBy)
            .Must(x => string.IsNullOrEmpty(x) || new[] { "title", "price", "category", "rating", "createdat" }.Contains(x.ToLower()))
            .When(x => !string.IsNullOrEmpty(x.OrderBy))
            .WithMessage("OrderBy deve ser: title, price, category, rating ou createdat");

        RuleFor(x => x)
            .Must(x => !x.MinPrice.HasValue || !x.MaxPrice.HasValue || x.MinPrice <= x.MaxPrice)
            .WithMessage("MinPrice deve ser menor ou igual a MaxPrice");
    }
}
