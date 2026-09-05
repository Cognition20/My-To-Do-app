using To_Do.DataAccess.Models;

namespace To_Do.DataAccess.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<Category?> GetByNameAsync(string categoryName, Guid userId, CancellationToken cancellationToken);
    Task AddAsync(Category category, CancellationToken cancellationToken);
    Task UpdateAsync(Category category, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid userId , Guid category, CancellationToken cancellationToken);
    Task<(List<Category> Items, int TotalCount)> GetAllAsync(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}