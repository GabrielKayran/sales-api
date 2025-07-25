using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSaleHandler> _logger;

    public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper, ILogger<CreateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingSale = await _saleRepository.GetBySaleNumberAsync(command.SaleNumber, cancellationToken);
        if (existingSale != null)
            throw new InvalidOperationException($"Venda com número {command.SaleNumber} já existe");

        var sale = new Sale
        {
            SaleNumber = command.SaleNumber,
            SaleDate = command.SaleDate,
            Customer = command.Customer,
            Branch = command.Branch
        };

        foreach (var itemRequest in command.Items)
        {
            var saleItem = new SaleItem
            {
                Product = itemRequest.Product,
                Quantity = itemRequest.Quantity,
                UnitPrice = itemRequest.UnitPrice,
                SaleId = sale.Id
            };
            
            saleItem.CalculateDiscount();
            sale.Items.Add(saleItem);
        }

        sale.CalculateTotalAmount();

        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);

        var saleCreatedEvent = new SaleCreatedEvent(
            createdSale.Id,
            createdSale.SaleNumber, 
            createdSale.Customer,
            createdSale.Branch,
            createdSale.TotalAmount,
            createdSale.Items.Count
        );

        _logger.LogInformation("Venda criada: {@SaleCreatedEvent}", saleCreatedEvent);

        return _mapper.Map<CreateSaleResult>(createdSale);
    }
} 