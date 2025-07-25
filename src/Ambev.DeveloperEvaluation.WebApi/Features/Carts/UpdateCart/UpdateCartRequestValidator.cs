using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;

public class UpdateCartRequestValidator : AbstractValidator<UpdateCartRequest>
{
    public UpdateCartRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id é obrigatório");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId é obrigatório");

        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Date é obrigatório")
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1))
            .WithMessage("Date não pode ser no futuro");

        RuleFor(x => x.Products)
            .NotNull()
            .WithMessage("Products é obrigatório")
            .Must(products => products.Count > 0)
            .WithMessage("Carrinho deve ter pelo menos um produto");

        RuleForEach(x => x.Products)
            .SetValidator(new UpdateCartProductValidator());
    }
}

public class UpdateCartProductValidator : AbstractValidator<UpdateCartProductDto>
{
    public UpdateCartProductValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("ProductId é obrigatório");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity deve ser maior que 0")
            .LessThanOrEqualTo(1000)
            .WithMessage("Quantity não pode exceder 1000 unidades");
    }
}
