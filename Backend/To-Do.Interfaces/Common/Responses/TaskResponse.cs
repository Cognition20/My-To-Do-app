namespace To_Do.Interfaces.Common.Responses;

public record TaskResponse(
    Guid Id,
    string Title,
    string? Description,
    Guid? CategoryId,
    string CategoryName,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt,
    bool IsCompleted
    );