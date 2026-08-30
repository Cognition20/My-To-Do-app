using To_Do.Interfaces.Common.Requests;
using To_Do.Interfaces.Common.Responses;
using ErrorOr;

namespace To_Do.Interfaces.Services.TaskManager;

public interface IToDoService
{
    Task<ErrorOr<TaskResponse>> Create(CreateTaskRequest request);

    Task<ErrorOr<PagedResponse<TaskResponse>>> GetAll(int pageNumber, int pageSize);

    Task<ErrorOr<PagedResponse<TaskResponse>>>GetBySearch(SearchTaskRequest request);

    Task<ErrorOr<TaskResponse>> Update(UpdateTaskRequest request, Guid taskId);

    Task<ErrorOr<Deleted>> Delete(Guid id);
}