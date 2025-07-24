using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ISaleRepository
{
    Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<Sale>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IQueryable<Sale>> GetSalesAsync(CancellationToken cancellationToken = default);
    Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Sale>> GetSalesByCustomerAsync(string customer, CancellationToken cancellationToken = default);
    Task<IEnumerable<Sale>> GetSalesByBranchAsync(string branch, CancellationToken cancellationToken = default);
    Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}