using To_Do.Interfaces.Common.Responses;
using ErrorOr;
using To_Do.Interfaces.Common.Requests;

namespace To_Do.Interfaces.Services.Category;

public interface ICategoryService
{
    Task<ErrorOr<CategoryResponse>> Create(CategoryRequest categoryRequest, CancellationToken cancellationToken);
    Task<ErrorOr<CategoryResponse>> Update(Guid id, CategoryRequest categoryRequest, CancellationToken cancellationToken);
    Task<ErrorOr<Deleted>> Delete(Guid id, CancellationToken cancellationToken);
    Task<ErrorOr<PagedResponse<CategoryResponse>>> GetAll(int pageNumber, int pageSize, CancellationToken cancellationToken);
}