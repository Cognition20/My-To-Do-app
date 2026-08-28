using To_Do.DataAccess.Models;

namespace To_Do.DataAccess.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(Category category);
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(Guid id);
}