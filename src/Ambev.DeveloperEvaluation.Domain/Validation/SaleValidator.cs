using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        RuleFor(sale => sale.SaleNumber)
            .NotEmpty()
            .WithMessage("Número da venda é obrigatório")
            .MaximumLength(50)
            .WithMessage("Número da venda não pode exceder 50 caracteres");

        RuleFor(sale => sale.Customer)
            .NotEmpty()
            .WithMessage("Cliente é obrigatório")
            .MaximumLength(100)
            .WithMessage("Nome do cliente não pode exceder 100 caracteres");

        RuleFor(sale => sale.Branch)
            .NotEmpty()
            .WithMessage("Filial é obrigatória")
            .MaximumLength(100)
            .WithMessage("Nome da filial não pode exceder 100 caracteres");

        RuleFor(sale => sale.SaleDate)
            .NotEmpty()
            .WithMessage("Data da venda é obrigatória")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Data da venda não pode ser futura");

        RuleFor(sale => sale.Items)
            .NotEmpty()
            .WithMessage("Venda deve ter pelo menos um item");

        RuleForEach(sale => sale.Items)
            .SetValidator(new SaleItemValidator());
    }
} 