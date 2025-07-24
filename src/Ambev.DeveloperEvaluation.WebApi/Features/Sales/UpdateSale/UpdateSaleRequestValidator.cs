using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
{
    public UpdateSaleRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id da venda é obrigatório");

        RuleFor(x => x.SaleNumber)
            .NotEmpty()
            .WithMessage("Número da venda é obrigatório")
            .Length(1, 50)
            .WithMessage("Número da venda deve ter entre 1 e 50 caracteres");

        RuleFor(x => x.SaleDate)
            .NotEmpty()
            .WithMessage("Data da venda é obrigatória")
            .LessThanOrEqualTo(DateTime.Now)
            .WithMessage("Data da venda não pode ser futura");

        RuleFor(x => x.Customer)
            .NotEmpty()
            .WithMessage("Cliente é obrigatório")
            .Length(1, 100)
            .WithMessage("Nome do cliente deve ter entre 1 e 100 caracteres");

        RuleFor(x => x.Branch)
            .NotEmpty()
            .WithMessage("Filial é obrigatória")
            .Length(1, 100)
            .WithMessage("Nome da filial deve ter entre 1 e 100 caracteres");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("A venda deve ter pelo menos um item");

        RuleForEach(x => x.Items).SetValidator(new UpdateSaleItemDtoValidator());
    }
}

public class UpdateSaleItemDtoValidator : AbstractValidator<UpdateSaleItemDto>
{
    public UpdateSaleItemDtoValidator()
    {
        RuleFor(x => x.Product)
            .NotEmpty()
            .WithMessage("Nome do produto é obrigatório")
            .Length(1, 100)
            .WithMessage("Nome do produto deve ter entre 1 e 100 caracteres");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantidade deve ser maior que zero")
            .LessThanOrEqualTo(20)
            .WithMessage("Não é possível vender mais de 20 itens idênticos");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Preço unitário deve ser maior que zero");
    }
}
