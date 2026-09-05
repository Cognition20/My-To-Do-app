using To_Do.DataAccess.Models;
using To_Do.Interfaces.Common.Responses;

namespace To_Do.Services.Mapping;

public static class TaskMappingExtensions
{
    public static TaskResponse ToResponse(this ToDo task)
    {
        return new TaskResponse(
            task.Id,
            task.Title,
            task.Description,
            task.CategoryId,
            task.Category?.Name ?? "No category",
            task.CreatedAtUtc.ToLocalTime(),
            task.UpdatedAtUtc.HasValue ? task.UpdatedAtUtc.Value.ToLocalTime() : null,
            task.IsCompleted);
    }
    
    public static IEnumerable<TaskResponse> ToResponseList(
        this IEnumerable<ToDo> tasks)
    {
        return tasks.Select(x => x.ToResponse());
    }
}