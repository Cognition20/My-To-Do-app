using Microsoft.EntityFrameworkCore;
using To_Do.DataAccess.ApplicationDbContext;
using To_Do.DataAccess.Models;
using To_Do.DataAccess.Repositories.Interfaces;

namespace To_Do.DataAccess.Repositories.Implementation;

public class CategoryRepository(AppDbContext dbContext) : ICategoryRepository
{
    public async Task<Category?> GetByNameAsync(string categoryName, Guid userId, CancellationToken cancellationToken)
    {
        return await dbContext.Categories.FirstOrDefaultAsync(x => x.Name == categoryName && x.UserId == userId, cancellationToken);
    }

    public async Task AddAsync(Category category, CancellationToken cancellationToken)
    {
        await dbContext.Categories.AddAsync(category, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Category category, CancellationToken cancellationToken)
    {
        dbContext.Categories.Update(category);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid id, CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .Include(c => c.ToDos)
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId, cancellationToken);

        if (category is null)
            return false;
        
        
        foreach (var toDo in category.ToDos)
        {
            toDo.CategoryId = null;
        }

        dbContext.Categories.Remove(category);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<(List<Category> Items, int TotalCount)> GetAllAsync(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = dbContext.Categories
            .AsNoTracking()
            .Where(t => t.UserId == userId);
        
        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        
        return (items, totalCount);
    }

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Categories.FindAsync(id, cancellationToken);
    }
    
}