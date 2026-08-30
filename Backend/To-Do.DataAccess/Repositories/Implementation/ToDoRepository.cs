using ErrorOr;
using Microsoft.EntityFrameworkCore;
using To_Do.DataAccess.ApplicationDbContext;
using To_Do.DataAccess.Models;
using To_Do.DataAccess.Repositories.Interfaces;

namespace To_Do.DataAccess.Repositories.Implementation;

public class ToDoRepository(AppDbContext dbContext) : IToDoRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task AddAsync(ToDo toDo)
    {
        await _dbContext.ToDos.AddAsync(toDo);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ToDo?> GetByIdAsync(Guid userId, Guid id)
    {
        return await _dbContext.ToDos
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
    }

    public async Task<(List<ToDo> Items, int TotalCount)> GetAllAsync(Guid userId, int pageNumber, int pageSize)
    {
        var query = _dbContext.ToDos
            .AsNoTracking()
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAtUtc);
        
        var totalCount = await query.CountAsync();
        
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return (items, totalCount);
    }

    public async Task<List<ToDo>> GetByCategoryAsync(Guid userId , Guid categoryId)
    {
        return await _dbContext.ToDos
            .AsNoTracking()
            .Where(t => t.CategoryId == categoryId && t.UserId == userId)
            .ToListAsync();
    }

    public async Task<(List<ToDo> Items, int TotalCount)>  
        SearchAsync(Guid userId, string? categorySearch, Guid? categoryId, int pageNumber, int pageSize)
    {
        if (string.IsNullOrWhiteSpace(categorySearch) && !categoryId.HasValue)
            return ([], 0);
        
        var query = _dbContext.ToDos
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
        var totalCount = await query.CountAsync();
        
        var items = await query
            .OrderByDescending(t => t.CreatedAtUtc)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task UpdateAsync(ToDo toDo)
    {
        _dbContext.ToDos.Update(toDo);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid id)
    {
        var task = await _dbContext.ToDos
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (task is null)
            return false;

        _dbContext.ToDos.Remove(task);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}