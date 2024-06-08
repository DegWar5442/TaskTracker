using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Common.Exceptions;
using TaskTracker.Data;
using TaskTracker.Data.Entities;
using TaskTracker.Web.Services.Auth;
using TaskTracker.Web.Services.Common.Dtos;
using TaskTracker.Web.Services.Tasks.Dtos;

namespace TaskTracker.Web.Services.Tasks;

public interface ITaskService
{
    public Task<TaskDto> CreateTask(CreateTaskDto createTaskDto, CancellationToken cancellationToken);
    public Task<PagedResultDto<TaskDto>> GetAllTasks(PagedRequestDto pagedRequestDto, CancellationToken cancellationToken);
    public Task<TaskDto> GetTaskById(Guid taskId, CancellationToken cancellationToken);
    public Task DeleteTask(Guid taskId, CancellationToken cancellationToken);
    public Task<TaskDto> UpdateTask(UpdateTaskDto updateTaskDto, CancellationToken cancellationToken);
}

public class TaskService(IDbContext dbContext, IMapper mapper, ICurrentIdentity currentIdentity) : ITaskService
{
    private DbSet<FolderEntity> Folders => dbContext.Set<FolderEntity>();
    private DbSet<TaskEntity> Tasks => dbContext.Set<TaskEntity>();

    public async Task<TaskDto> CreateTask(CreateTaskDto createTaskDto, CancellationToken cancellationToken)
    {
        var folder = await Folders.FirstOrDefaultAsync(x => x.Id == createTaskDto.FolderId, cancellationToken) ?? throw new BadRequestException("Папку не знайдено");

        if (folder.OwnerId != currentIdentity.GetUserGuid())
        {
            throw new ForbiddenException("У вас немає дозволу створювати завдання у даній папці");
        }

        var taskEntity = mapper.Map<TaskEntity>(createTaskDto);

        Tasks.Add(taskEntity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<TaskDto>(taskEntity);
    }

    public async Task<PagedResultDto<TaskDto>> GetAllTasks(PagedRequestDto pagedRequestDto, CancellationToken cancellationToken)
    {
        var query = Tasks;

        var totalItems = await query.CountAsync(cancellationToken);

        var tasks = await query
            .Where(x => x.Folder.OwnerId == currentIdentity.GetUserGuid())
            .Include(x => x.Folder)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pagedRequestDto.Page - 1) * pagedRequestDto.PageSize)
            .Take(pagedRequestDto.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResultDto<TaskDto>(
            pagedRequestDto.Page,
            pagedRequestDto.PageSize,
            totalItems,
            mapper.Map<List<TaskDto>>(tasks));
    }

    public async Task<TaskDto> GetTaskById(Guid taskId, CancellationToken cancellationToken)
    {
        var taskEntity = await Tasks.Include(x => x.Folder).FirstOrDefaultAsync(x => x.Id == taskId, cancellationToken)
            ?? throw new BadRequestException("Завдання не знайдено");

        return taskEntity.Folder.OwnerId != currentIdentity.GetUserGuid()
            ? throw new ForbiddenException("У вас немає дозволу на перегляд цього завдання")
            : mapper.Map<TaskDto>(taskEntity);
    }

    public async Task DeleteTask(Guid taskId, CancellationToken cancellationToken)
    {
        var taskEntity = await Tasks.Include(x => x.Folder).FirstOrDefaultAsync(x => x.Id == taskId, cancellationToken)
            ?? throw new BadRequestException("Завдання не знайдено");

        if (taskEntity.Folder.OwnerId != currentIdentity.GetUserGuid())
        {
            throw new ForbiddenException("У вас немає дозволу на видалення цього завдання");
        }

        Tasks.Remove(taskEntity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<TaskDto> UpdateTask(UpdateTaskDto updateTaskDto, CancellationToken cancellationToken)
    {
        var taskEntity = await Tasks.Include(x => x.Folder).FirstOrDefaultAsync(x => x.Id == updateTaskDto.Id, cancellationToken)
            ?? throw new BadRequestException("Завдання не знайдено");

        if (taskEntity.FolderId != updateTaskDto.FolderId)
        {
            var folder = await Folders.FirstOrDefaultAsync(x => x.Id == updateTaskDto.FolderId, cancellationToken)
                ?? throw new BadRequestException("Папку не знайдено");
        }

        if (taskEntity.Folder.OwnerId != currentIdentity.GetUserGuid())
        {
            throw new ForbiddenException("You don't have permission to modify this task");
        }

        mapper.Map(updateTaskDto, taskEntity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<TaskDto>(taskEntity);
    }
}
