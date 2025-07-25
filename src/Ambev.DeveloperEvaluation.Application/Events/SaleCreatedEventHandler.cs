using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Application.Events;

/// <summary>
/// Handler for SaleCreatedEvent
/// </summary>
public class SaleCreatedEventHandler : INotificationHandler<SaleCreatedEvent>
{
    private readonly ILogger<SaleCreatedEventHandler> _logger;

    public SaleCreatedEventHandler(ILogger<SaleCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(SaleCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Venda criada com sucesso! ID: {SaleId}, Número: {SaleNumber}, Cliente: {Customer}, Filial: {Branch}, Valor: {TotalAmount:C}, Itens: {ItemCount}",
            notification.SaleId,
            notification.SaleNumber,
            notification.Customer,
            notification.Branch,
            notification.TotalAmount,
            notification.ItemCount
        );
        
        await Task.CompletedTask;
    }
}
