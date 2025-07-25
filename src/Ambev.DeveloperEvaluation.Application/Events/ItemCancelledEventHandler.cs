using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Application.Events;

/// <summary>
/// Handler for ItemCancelledEvent
/// </summary>
public class ItemCancelledEventHandler : INotificationHandler<ItemCancelledEvent>
{
    private readonly ILogger<ItemCancelledEventHandler> _logger;

    public ItemCancelledEventHandler(ILogger<ItemCancelledEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(ItemCancelledEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Item cancelado! VendaID: {SaleId}, ItemID: {ItemId}, Produto: {Product}, Quantidade: {Quantity}, Preço Unitário: {UnitPrice:C}, Valor Total: {TotalAmount:C}",
            notification.SaleId,
            notification.ItemId,
            notification.Product,
            notification.Quantity,
            notification.UnitPrice,
            notification.TotalAmount
        );
        
        await Task.CompletedTask;
    }
}
