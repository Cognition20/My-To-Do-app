namespace To_Do.Interfaces.Common.Responses;

public record PagedResponse<T>(
    List<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);