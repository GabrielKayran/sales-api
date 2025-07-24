using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Título do produto é obrigatório")
            .Length(1, 200)
            .WithMessage("Título deve ter entre 1 e 200 caracteres");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Preço deve ser maior que zero");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Descrição do produto é obrigatória")
            .Length(1, 1000)
            .WithMessage("Descrição deve ter entre 1 e 1000 caracteres");

        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Categoria do produto é obrigatória")
            .Length(1, 100)
            .WithMessage("Categoria deve ter entre 1 e 100 caracteres");

        RuleFor(x => x.Image)
            .MaximumLength(500)
            .WithMessage("URL da imagem deve ter no máximo 500 caracteres");

        RuleFor(x => x.Rating)
            .SetValidator(new CreateProductRatingValidator());
    }
}

public class CreateProductRatingValidator : AbstractValidator<CreateProductRatingRequest>
{
    public CreateProductRatingValidator()
    {
        RuleFor(x => x.Rate)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(5)
            .WithMessage("Rating deve estar entre 0 e 5");

        RuleFor(x => x.Count)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Contagem de ratings deve ser maior ou igual a zero");
    }
}
