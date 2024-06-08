using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Web.Services.Common.Dtos;
using TaskTracker.Web.Services.Tasks;
using TaskTracker.Web.Services.Tasks.Dtos;

namespace TaskTracker.Web.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TasksController(ITaskService taskService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<TaskDto> CreateTask([FromBody] CreateTaskDto createTaskDto, CancellationToken cancellationToken)
    {
        return await taskService.CreateTask(createTaskDto, cancellationToken);
    }

    [HttpGet("all")]
    public async Task<PagedResultDto<TaskDto>> GetAllTasks([FromQuery] PagedRequestDto pagedRequestDto, CancellationToken cancellationToken)
    {
        return await taskService.GetAllTasks(pagedRequestDto, cancellationToken);
    }

    [HttpGet("{taskId}")]
    public async Task<TaskDto> GetTask([FromRoute] Guid taskId, CancellationToken cancellationToken)
    {
        return await taskService.GetTaskById(taskId, cancellationToken);
    }

    [HttpDelete("delete/{taskId}")]
    public async Task DeleteTask([FromRoute] Guid taskId, CancellationToken cancellationToken)
    {
        await taskService.DeleteTask(taskId, cancellationToken);
    }

    [HttpPut("update")]
    public async Task<TaskDto> UpdateTask([FromBody] UpdateTaskDto updateTaskDto, CancellationToken cancellationToken)
    {
        return await taskService.UpdateTask(updateTaskDto, cancellationToken);
    }
}