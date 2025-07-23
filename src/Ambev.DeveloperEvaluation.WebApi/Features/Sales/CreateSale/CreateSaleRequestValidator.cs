using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    public CreateSaleRequestValidator()
    {
        RuleFor(request => request.SaleNumber)
            .NotEmpty()
            .WithMessage("Número da venda é obrigatório")
            .MaximumLength(50)
            .WithMessage("Número da venda não pode exceder 50 caracteres");

        RuleFor(request => request.Customer)
            .NotEmpty()
            .WithMessage("Cliente é obrigatório")
            .MaximumLength(100)
            .WithMessage("Nome do cliente não pode exceder 100 caracteres");

        RuleFor(request => request.Branch)
            .NotEmpty()
            .WithMessage("Filial é obrigatória")
            .MaximumLength(100)
            .WithMessage("Nome da filial não pode exceder 100 caracteres");

        RuleFor(request => request.SaleDate)
            .NotEmpty()
            .WithMessage("Data da venda é obrigatória")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Data da venda não pode ser futura");

        RuleFor(request => request.Items)
            .NotEmpty()
            .WithMessage("Venda deve ter pelo menos um item");

        RuleForEach(request => request.Items)
            .SetValidator(new CreateSaleItemValidator());
    }
}

public class CreateSaleItemValidator : AbstractValidator<CreateSaleItemDto>
{
    public CreateSaleItemValidator()
    {
        RuleFor(item => item.Product)
            .NotEmpty()
            .WithMessage("Nome do produto é obrigatório")
            .MaximumLength(100)
            .WithMessage("Nome do produto não pode exceder 100 caracteres");

        RuleFor(item => item.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantidade deve ser maior que zero")
            .LessThanOrEqualTo(20)
            .WithMessage("Quantidade não pode exceder 20 itens");

        RuleFor(item => item.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Preço unitário deve ser maior que zero");
    }
} 