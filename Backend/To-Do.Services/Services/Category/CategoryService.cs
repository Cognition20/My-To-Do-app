using ErrorOr;
using To_Do.DataAccess.Common.Errors;
using To_Do.DataAccess.Repositories.Interfaces;
using To_Do.Interfaces.Common.Requests;
using To_Do.Interfaces.Common.Responses;
using To_Do.Interfaces.Services;
using To_Do.Interfaces.Services.Category;
using To_Do.Services.Mapping;

namespace To_Do.Services.Services.Category;

public class CategoryService(ICategoryRepository categoryRepository, ICurrentUserId currentUserId) : ICategoryService
{
    private const int MaxPageSize = 25;
    public async Task<ErrorOr<CategoryResponse>> Create(CategoryRequest categoryRequest)
    {
        var isExists = await categoryRepository.GetByNameAsync(categoryRequest.Name, currentUserId.UserId);

        if (isExists is not null)
        {
            return Errors.Category.AlreadyExists(categoryRequest.Name);
        }

        var category = new DataAccess.Models.Category
        {
            Id = Guid.NewGuid(),
            Name = categoryRequest.Name,
            UserId = currentUserId.UserId
        };
            
        await categoryRepository.AddAsync(category);

        return category.ToResponse();
    }

    public async Task<ErrorOr<CategoryResponse>> Update(Guid id, CategoryRequest categoryRequest)
    {
        var category = await categoryRepository.GetByIdAsync(id);
        
        if (category is null || category.UserId != currentUserId.UserId)
            return Errors.Category.NotFound(id);
        
        var duplicate = await categoryRepository.GetByNameAsync(categoryRequest.Name, currentUserId.UserId);
        if (duplicate is not null && duplicate.Id != id)
            return Errors.Category.AlreadyExists(categoryRequest.Name);
        
        category.Name = categoryRequest.Name;
        
        await categoryRepository.UpdateAsync(category);
        return category.ToResponse();
    }

    public async Task<ErrorOr<Deleted>> Delete(Guid id)
    {
        var deleted = await categoryRepository.DeleteAsync(currentUserId.UserId, id);
        
        if (!deleted)
            return Errors.Category.NotFound(id);

        return Result.Deleted;
    }

    public async Task<ErrorOr<PagedResponse<CategoryResponse>>> GetAll(int pageNumber, int pageSize)
    {
        pageNumber = Math.Max(1, pageNumber);
        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);
        
        var (categories, totalCount) = await categoryRepository.GetAllAsync(currentUserId.UserId, pageNumber, pageSize);

        return new PagedResponse<CategoryResponse>(
            categories.ToResponseList().ToList(),
            pageNumber,
            pageSize,
            totalCount,
            (int)Math.Ceiling(totalCount / (double)pageSize));
    }
}