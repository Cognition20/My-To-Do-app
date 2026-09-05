using To_Do.DataAccess.Models;

namespace To_Do.DataAccess.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<Category?> GetByNameAsync(string categoryName, Guid userId);
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task<bool> DeleteAsync(Guid userId , Guid category);
    Task<(List<Category> Items, int TotalCount)> GetAllAsync(Guid userId, int pageNumber, int pageSize);
    Task<Category?> GetByIdAsync(Guid id);
}