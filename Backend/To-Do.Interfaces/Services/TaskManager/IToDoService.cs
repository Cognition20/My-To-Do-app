using To_Do.Interfaces.Common.Requests;
using To_Do.Interfaces.Common.Responses;
using ErrorOr;

namespace To_Do.Interfaces.Services.TaskManager;

public interface IToDoService
{
    Task<ErrorOr<TaskResponse>> Create(CreateTaskRequest request,  CancellationToken cancellationToken);

    Task<ErrorOr<PagedResponse<TaskResponse>>> GetAll(int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<ErrorOr<PagedResponse<TaskResponse>>>GetBySearch(SearchTaskRequest request, CancellationToken cancellationToken);

    Task<ErrorOr<TaskResponse>> Update(UpdateTaskRequest request, Guid taskId,  CancellationToken cancellationToken);

    Task<ErrorOr<Deleted>> Delete(Guid id,  CancellationToken cancellationToken);
}