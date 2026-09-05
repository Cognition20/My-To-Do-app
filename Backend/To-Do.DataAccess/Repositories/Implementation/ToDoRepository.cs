using Microsoft.EntityFrameworkCore;
using To_Do.DataAccess.ApplicationDbContext;
using To_Do.DataAccess.Models;
using To_Do.DataAccess.Repositories.Interfaces;

namespace To_Do.DataAccess.Repositories.Implementation;

public class ToDoRepository(AppDbContext dbContext) : IToDoRepository
{
    public async Task AddAsync(ToDo toDo, CancellationToken cancellationToken)
    {
        await dbContext.ToDos.AddAsync(toDo, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<ToDo?> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.ToDos
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId, cancellationToken);
    }

    public async Task<(List<ToDo> Items, int TotalCount)> GetAllAsync(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = dbContext.ToDos
            .AsNoTracking()
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAtUtc);
        
        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .Include(t => t.Category)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        
        return (items, totalCount);
    }

    public async Task<List<ToDo>> GetByCategoryAsync(Guid userId , Guid categoryId, CancellationToken cancellationToken)
    {
        return await dbContext.ToDos
            .AsNoTracking()
            .Where(t => t.CategoryId == categoryId && t.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<(List<ToDo> Items, int TotalCount)>  
        SearchAsync(Guid userId, string? categorySearch, Guid? categoryId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
       /* if (string.IsNullOrWhiteSpace(categorySearch) && !categoryId.HasValue)
            return ([], 0);*/
        
        var query = dbContext.ToDos
            .AsNoTracking()
            .Include(t => t.Category)
            .Where(t => t.UserId == userId);

        if (!string.IsNullOrWhiteSpace(categorySearch))
        {
            query = query.Where(t =>
                t.Category != null && t.Category.Name.Contains(categorySearch));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(t => t.CategoryId == categoryId.Value);
        }
        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .OrderByDescending(t => t.CreatedAtUtc)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task UpdateAsync(ToDo toDo, CancellationToken cancellationToken)
    {
        dbContext.ToDos.Update(toDo);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid id, CancellationToken cancellationToken)
    {
        var task = await dbContext.ToDos
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId, cancellationToken);

        if (task is null)
            return false;

        dbContext.ToDos.Remove(task);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}