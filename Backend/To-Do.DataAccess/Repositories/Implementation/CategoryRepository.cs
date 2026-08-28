using Microsoft.EntityFrameworkCore;
using To_Do.DataAccess.ApplicationDbContext;
using To_Do.DataAccess.Models;
using To_Do.DataAccess.Repositories.Interfaces;

namespace To_Do.DataAccess.Repositories.Implementation;

public class CategoryRepository(AppDbContext dbContext) : ICategoryRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task AddAsync(Category category)
    {
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category category)
    {
        _dbContext.Categories.Update(category);

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Category category)
    {
        _dbContext.Categories.Remove(category);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _dbContext.Categories.AsNoTracking().ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Categories.FindAsync(id);
    }
}