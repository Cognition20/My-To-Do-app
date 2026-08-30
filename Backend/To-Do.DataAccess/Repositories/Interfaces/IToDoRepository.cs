using To_Do.DataAccess.Models;

namespace To_Do.DataAccess.Repositories.Interfaces;

public interface IToDoRepository
{
    Task AddAsync(ToDo toDo);
    Task<ToDo?> GetByIdAsync(Guid userId, Guid id);
    Task<(List<ToDo> Items, int TotalCount)>GetAllAsync(Guid userId, int pageNumber, int pageSize);
    Task<List<ToDo>> GetByCategoryAsync(Guid userId, Guid categoryId);
    Task<(List<ToDo> Items, int TotalCount)> SearchAsync(Guid userId, string? search, Guid? categoryId, int pageNumber, int pageSize);
    Task UpdateAsync(ToDo toDo);
    Task<bool> DeleteAsync(Guid userId, Guid toDo);
}