using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Application.Events;

/// <summary>
/// Handler for SaleModifiedEvent
/// </summary>
public class SaleModifiedEventHandler : INotificationHandler<SaleModifiedEvent>
{
    private readonly ILogger<SaleModifiedEventHandler> _logger;

    public SaleModifiedEventHandler(ILogger<SaleModifiedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(SaleModifiedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Venda modificada! ID: {SaleId}, Número: {SaleNumber}, Tipo: {ModificationType}, Valor Anterior: {PreviousAmount:C}, Novo Valor: {TotalAmount:C}",
            notification.SaleId,
            notification.SaleNumber,
            notification.ModificationType,
            notification.PreviousAmount,
            notification.TotalAmount
        );

        await Task.CompletedTask;
    }
}
