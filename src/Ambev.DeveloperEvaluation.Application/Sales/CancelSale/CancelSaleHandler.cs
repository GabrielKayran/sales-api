using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResponse>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<CancelSaleHandler> _logger;

    public CancelSaleHandler(ISaleRepository saleRepository, ILogger<CancelSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _logger = logger;
    }

    public async Task<CancelSaleResponse> Handle(CancelSaleCommand command, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        
        if (sale == null)
            throw new KeyNotFoundException($"Venda com ID {command.Id} não encontrada");

        if (sale.Status == Domain.Enums.SaleStatus.Cancelled)
            throw new InvalidOperationException("Venda já está cancelada");

        sale.Cancel();
        await _saleRepository.UpdateAsync(sale, cancellationToken);

        var saleCancelledEvent = new SaleCancelledEvent(
            sale.Id,
            sale.SaleNumber,
            command.Reason,
            sale.TotalAmount 
        );

        _logger.LogInformation("Venda cancelada: {@SaleCancelledEvent}", saleCancelledEvent);

        return new CancelSaleResponse
        {
            Success = true,
            Message = "Venda cancelada com sucesso"
        };
    }
} 