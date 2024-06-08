using TaskTracker.Web.Services.Tasks.Dtos;

namespace TaskTracker.Web.Services.Folder.Dtos;

public record FolderDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public List<TaskDto> Tasks { get; set; } = [];
}
