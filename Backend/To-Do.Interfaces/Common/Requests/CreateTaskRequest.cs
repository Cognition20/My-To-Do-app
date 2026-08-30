namespace To_Do.Interfaces.Common.Requests;

public record CreateTaskRequest(
    string Title,
    string? Description,
    Guid? CategoryId);