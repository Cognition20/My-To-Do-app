using ErrorOr;
using To_Do.DataAccess.Common.Errors;
using To_Do.DataAccess.Models;
using To_Do.DataAccess.Repositories.Interfaces;
using To_Do.Interfaces.Common.Requests;
using To_Do.Interfaces.Common.Responses;
using To_Do.Interfaces.Services;
using To_Do.Interfaces.Services.TaskManager;
using To_Do.Services.Mapping;

namespace To_Do.Services.Services.ToDoServices;

public class ToDoService(IToDoRepository toDoRepository, ICurrentUserId currentUserId, ICategoryRepository categoryRepository) : IToDoService
{
    private const int MaxPageSize = 10;
    
    public async Task<ErrorOr<TaskResponse>> Create(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        if (request.CategoryId.HasValue)
        {
            var category = await categoryRepository.GetByIdAsync(request.CategoryId.Value, cancellationToken);

            if (category is null || category.UserId != currentUserId.UserId)
                return Errors.Category.NotFound(request.CategoryId.Value);
        }
        
        var task = new ToDo
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            CategoryId = request.CategoryId,
            UserId = currentUserId.UserId,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            UpdatedAtUtc = null,
            IsCompleted = false
        };
        
        await toDoRepository.AddAsync(task, cancellationToken);

        return task.ToResponse();
    }

    public async Task<ErrorOr<PagedResponse<TaskResponse>>> GetAll(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        pageNumber = Math.Max(1, pageNumber);
        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);
        
        var (items, totalCount) = await toDoRepository.GetAllAsync(currentUserId.UserId,  pageNumber, pageSize, cancellationToken);

        return new PagedResponse<TaskResponse>(
            items.ToResponseList().ToList(),
            pageNumber,
            pageSize,
            totalCount,
            (int)Math.Ceiling(totalCount / (double)pageSize));
    }

    public async Task<ErrorOr<PagedResponse<TaskResponse>>> GetBySearch(SearchTaskRequest request, CancellationToken cancellationToken)
    {
        var pageNumber = Math.Max(1, request.PageNumber);
        var pageSize = request.PageSize;
        
        var (items, totalCount) = await toDoRepository.SearchAsync(currentUserId.UserId, 
            request.CategoryName, 
            request.CategoryId, 
            request.PageNumber, 
            request.PageSize, 
            cancellationToken);
        
        return new PagedResponse<TaskResponse>(
            items.ToResponseList().ToList(),
            pageNumber,
            pageSize,
            totalCount,
            (int)Math.Ceiling(totalCount / (double)pageSize));
    }

    public async Task<ErrorOr<TaskResponse>> Update(UpdateTaskRequest request, Guid taskId, CancellationToken cancellationToken)
    {
        var task = await toDoRepository.GetByIdAsync(currentUserId.UserId, taskId, cancellationToken);

        if (task is null || task.UserId != currentUserId.UserId)
            return Errors.ToDo.NotFound();

        if (request.CategoryId.HasValue)
        {
            var category = await categoryRepository.GetByIdAsync(request.CategoryId.Value,  cancellationToken);
            if (category is null || category.UserId != currentUserId.UserId)
                return Errors.Category.NotFound(request.CategoryId.Value);
        }

        task.Title = request.Title;
        task.Description = request.Description;
        task.CategoryId = request.CategoryId;
        task.UpdatedAtUtc = DateTimeOffset.UtcNow;
        task.IsCompleted = request.IsCompleted ?? false;

        await toDoRepository.UpdateAsync(task, cancellationToken);

        return task.ToResponse();
        
    }

    public async Task<ErrorOr<Deleted>> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await toDoRepository.DeleteAsync(currentUserId.UserId, id, cancellationToken);

        if (!deleted)
            return Errors.ToDo.NotFound();

        return Result.Deleted;
    }
}