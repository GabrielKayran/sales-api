using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Common.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingSale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
        if (existingSale == null)
            throw new KeyNotFoundException($"Venda com ID {request.Id} não foi encontrada");

        if (existingSale.Status == Domain.Enums.SaleStatus.Cancelled)
            throw new InvalidOperationException("Não é possível atualizar uma venda cancelada");

        if (existingSale.SaleNumber != request.SaleNumber)
        {
            var existingWithSameNumber = await _saleRepository.GetBySaleNumberAsync(request.SaleNumber, cancellationToken);
            if (existingWithSameNumber != null && existingWithSameNumber.Id != request.Id)
                throw new InvalidOperationException("Já existe uma venda com este número");
        }

        existingSale.SaleNumber = request.SaleNumber;
        existingSale.SaleDate = request.SaleDate;
        existingSale.Customer = request.Customer;
        existingSale.Branch = request.Branch;

        existingSale.Items.Clear();

        foreach (var itemRequest in request.Items)
        {
            var saleItem = new SaleItem
            {
                Id = itemRequest.Id ?? Guid.NewGuid(),
                SaleId = existingSale.Id,
                Product = itemRequest.Product,
                Quantity = itemRequest.Quantity,
                UnitPrice = itemRequest.UnitPrice,
                Sale = existingSale
            };

            existingSale.AddItem(saleItem);
        }

        existingSale.CalculateTotalAmount();

        var updatedSale = await _saleRepository.UpdateAsync(existingSale, cancellationToken);

        return _mapper.Map<UpdateSaleResult>(updatedSale);
    }
}
