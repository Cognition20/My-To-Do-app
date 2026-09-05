using To_Do.Interfaces.Common.Responses;
using ErrorOr;
using To_Do.Interfaces.Common.Requests;

namespace To_Do.Interfaces.Services.Category;

public interface ICategoryService
{
    Task<ErrorOr<CategoryResponse>> Create(CategoryRequest categoryRequest);
    Task<ErrorOr<CategoryResponse>> Update(Guid id, CategoryRequest categoryRequest);
    Task<ErrorOr<Deleted>> Delete(Guid id);
    Task<ErrorOr<PagedResponse<CategoryResponse>>> GetAll(int pageNumber, int pageSize);
}