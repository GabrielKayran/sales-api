using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

public class CancelSaleRequestValidator : AbstractValidator<CancelSaleRequest>
{
    public CancelSaleRequestValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty()
            .WithMessage("ID da venda é obrigatório");

        RuleFor(request => request.Reason)
            .MaximumLength(500)
            .WithMessage("Motivo do cancelamento não pode exceder 500 caracteres");
    }
} 