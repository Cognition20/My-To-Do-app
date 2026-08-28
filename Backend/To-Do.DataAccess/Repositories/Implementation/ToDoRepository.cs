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

    public async Task<List<ToDo>> GetAllAsync()
    {
        return await _dbContext.ToDos.AsNoTracking().ToListAsync();
        
    }

    public async Task<List<ToDo>> GetByCategoryAsync(Guid categoryId)
    {
        var toDos = await _dbContext.ToDos.Where(t => t.CategoryId == categoryId).ToListAsync();
        return toDos;
    }

    public async Task<List<ToDo>> SearchAsync(string? search, Guid? categoryId)
    {
        var query = _dbContext.ToDos.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(t =>
                t.Title.Contains(search) ||
                ( t.Description != null && t.Description.Contains(search)));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(x =>
                x.CategoryId == categoryId.Value);
        }

        return await query.ToListAsync();

    }

    public async Task UpdateAsync(ToDo toDo)
    {
        _dbContext.ToDos.Update(toDo);

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(ToDo toDo)
    {
        _dbContext.ToDos.Remove(toDo);

        await _dbContext.SaveChangesAsync();
        
    }
}