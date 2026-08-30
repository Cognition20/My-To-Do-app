namespace To_Do.Interfaces.Common.Requests;

public record UpdateTaskRequest(
    string Title,
    string? Description,
    Guid? CategoryId,
    bool? IsCompleted);
