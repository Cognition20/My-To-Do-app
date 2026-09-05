using Microsoft.AspNetCore.Mvc;
using To_Do.Interfaces.Common.Requests;
using To_Do.Interfaces.Services.TaskManager;


namespace To_Do.Controllers;

[Route("tasks")]
public class TaskController(IToDoService toDoService) : ApiController
{
    [HttpGet("getTasks")]
    public async Task<IActionResult> Get([FromQuery] int pageNumber, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        var result = await toDoService.GetAll(pageNumber, pageSize, cancellationToken);
        
        return result.Match(
            getTasksResult => Ok(getTasksResult),
            errors => Problem(errors));
    }
    
    [HttpPost("search")]
    public async Task<IActionResult> GetByCategory(SearchTaskRequest searchTaskRequest, CancellationToken cancellationToken)
    {
        var result = await toDoService.GetBySearch(searchTaskRequest, cancellationToken);
        
        return result.Match(
            getTasksByCatResult => Ok(getTasksByCatResult),
            errors => Problem(errors));
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateTaskRequest taskRequest, CancellationToken cancellationToken)
    {
        var result = await toDoService.Create(taskRequest, cancellationToken);
        
        return result.Match(
            createTaskResult => Ok(createTaskResult),
            errors => Problem(errors));
    }

    [HttpPatch("update/{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateTaskRequest taskRequest, CancellationToken cancellationToken)
    {
        var result = await toDoService.Update(taskRequest, id, cancellationToken);
        
        return result.Match(
            updateTaskResult => Ok(updateTaskResult),
            errors => Problem(errors));
    }

    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await toDoService.Delete(id, cancellationToken);
        
        return result.Match(
            deleteTaskResult => Ok(deleteTaskResult),
            errors => Problem(errors));
    }
}