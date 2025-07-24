using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title é obrigatório")
            .MaximumLength(200)
            .WithMessage("Title deve ter no máximo 200 caracteres");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price deve ser maior que 0");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description é obrigatório")
            .MaximumLength(1000)
            .WithMessage("Description deve ter no máximo 1000 caracteres");

        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Category é obrigatório")
            .MaximumLength(50)
            .WithMessage("Category deve ter no máximo 50 caracteres");

        RuleFor(x => x.Image)
            .NotEmpty()
            .WithMessage("Image é obrigatório")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("Image deve ser uma URL válida");

        RuleFor(x => x.Rating.Rate)
            .InclusiveBetween(0, 5)
            .WithMessage("Rating.Rate deve estar entre 0 e 5");

        RuleFor(x => x.Rating.Count)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Rating.Count deve ser maior ou igual a 0");
    }
}
