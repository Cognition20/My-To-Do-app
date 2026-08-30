using Microsoft.AspNetCore.Mvc;
using To_Do.Interfaces.Common.Requests;
using To_Do.Interfaces.Services.Category;

namespace To_Do.Controllers;

[Route("category")]
public class CategoryController(ICategoryService categoryService) : ApiController
{
    
    [HttpPost("create")]
    public async Task<IActionResult> Create(CategoryRequest categoryRequest, CancellationToken cancellationToken)
    {
        var result = await categoryService.Create(categoryRequest);
        
        return result.Match(
            createCategoryResult => Ok(createCategoryResult),
            errors => Problem(errors));
    }

    [HttpGet("get")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await categoryService.GetAll();
                
        return result.Match(
            getCategoryResult => Ok(getCategoryResult),
            errors => Problem(errors));
    }
    
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, CategoryRequest categoryRequest, CancellationToken cancellationToken)
    {
        var result = await categoryService.Update(id, categoryRequest);
        
        return result.Match(
            updateCategoryResult => Ok(updateCategoryResult),
            errors => Problem(errors));
    }

    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await categoryService.Delete(id);
        
        return result.Match(
            deleteCategoryResult => Ok(deleteCategoryResult),
            errors => Problem(errors));
    }
}