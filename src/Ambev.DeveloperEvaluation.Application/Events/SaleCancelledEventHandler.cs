using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Application.Events;

/// <summary>
/// Handler for SaleCancelledEvent
/// </summary>
public class SaleCancelledEventHandler : INotificationHandler<SaleCancelledEvent>
{
    private readonly ILogger<SaleCancelledEventHandler> _logger;

    public SaleCancelledEventHandler(ILogger<SaleCancelledEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(SaleCancelledEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogWarning(
            "Venda cancelada! ID: {SaleId}, Número: {SaleNumber}, Motivo: {Reason}, Valor Cancelado: {CancelledAmount:C}",
            notification.SaleId,
            notification.SaleNumber,
            notification.Reason,
            notification.CancelledAmount
        );
        
        await Task.CompletedTask;
    }
}
