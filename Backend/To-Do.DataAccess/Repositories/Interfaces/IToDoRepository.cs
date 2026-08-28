using To_Do.DataAccess.Models;

namespace To_Do.DataAccess.Repositories.Interfaces;

public interface IToDoRepository
{
    Task AddAsync(ToDo toDo);
    Task<List<ToDo>> GetAllAsync();
    Task<List<ToDo>> GetByCategoryAsync(Guid categoryId);
    Task<List<ToDo>> SearchAsync(string? search, Guid? categoryId);
    Task UpdateAsync(ToDo toDo);
    Task DeleteAsync(ToDo toDo);
}