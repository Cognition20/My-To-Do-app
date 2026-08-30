namespace To_Do.Interfaces.Common.Requests;

public record SearchTaskRequest(
    string? CategoryName,
    Guid? CategoryId,
    int PageNumber = 1,
    int PageSize = 20);