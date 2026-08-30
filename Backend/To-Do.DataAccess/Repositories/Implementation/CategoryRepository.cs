using Microsoft.EntityFrameworkCore;
using To_Do.DataAccess.ApplicationDbContext;
using To_Do.DataAccess.Models;
using To_Do.DataAccess.Repositories.Interfaces;
using To_Do.Interfaces.Common.Requests;

namespace To_Do.DataAccess.Repositories.Implementation;

public class CategoryRepository(AppDbContext dbContext) : ICategoryRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<Category?> GetByNameAsync(string categoryName, Guid userId)
    {
        return await _dbContext.Categories.FirstOrDefaultAsync(x => x.Name == categoryName && x.UserId == userId);
    }

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

    public async Task<bool> DeleteAsync(Guid userId, Guid id)
    {
        var category = await _dbContext.Categories
            .Include(c => c.ToDos)
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

        if (category is null)
            return false;
        
        
        foreach (var toDo in category.ToDos)
        {
            toDo.CategoryId = null;
        }

        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<List<Category>> GetAllAsync(Guid userId)
    {
        return await _dbContext.Categories
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Categories.FindAsync(id);
    }
}