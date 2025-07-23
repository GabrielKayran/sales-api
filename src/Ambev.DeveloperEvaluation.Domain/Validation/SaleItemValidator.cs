using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleItemValidator : AbstractValidator<SaleItem>
{
    public SaleItemValidator()
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

        RuleFor(item => item)
            .Must(ValidateDiscountRule)
            .WithMessage("Desconto aplicado está incorreto para a quantidade informada");
    }

    private bool ValidateDiscountRule(SaleItem item)
    {
        if (item.Quantity < 4 && item.Discount > 0)
            return false;

        if (item.Quantity >= 4 && item.Quantity < 10)
        {
            var expectedDiscount = (item.UnitPrice * item.Quantity) * 0.10m;
            return Math.Abs(item.Discount - expectedDiscount) < 0.01m;
        }

        if (item.Quantity >= 10 && item.Quantity <= 20)
        {
            var expectedDiscount = (item.UnitPrice * item.Quantity) * 0.20m;
            return Math.Abs(item.Discount - expectedDiscount) < 0.01m;
        }

        return true;
    }
} 