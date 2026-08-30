using To_Do.DataAccess.Models;
using To_Do.Interfaces.Common.Responses;

namespace To_Do.Services.Mapping;

public static class CategoryMapping
{
    public static CategoryResponse ToResponse(this Category category)
    {
        
        return new CategoryResponse(
            category.Id,
            category.Name
            );
    }
    
    public static IEnumerable<CategoryResponse> ToResponseList(
        this IEnumerable<Category> categories)
    {
        return categories.Select(c => c.ToResponse());
    }
}