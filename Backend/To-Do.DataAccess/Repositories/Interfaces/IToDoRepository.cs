using To_Do.DataAccess.Models;

namespace To_Do.DataAccess.Repositories.Interfaces;

public interface IToDoRepository
{
    Task AddAsync(ToDo toDo, CancellationToken cancellationToken);
    Task<ToDo?> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken);
    Task<(List<ToDo> Items, int TotalCount)>GetAllAsync(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<List<ToDo>> GetByCategoryAsync(Guid userId, Guid categoryId, CancellationToken cancellationToken);
    Task<(List<ToDo> Items, int TotalCount)> SearchAsync(Guid userId, string? search, Guid? categoryId, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task UpdateAsync(ToDo toDo, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid userId, Guid toDo, CancellationToken cancellationToken);
}