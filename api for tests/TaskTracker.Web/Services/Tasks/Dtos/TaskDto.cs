using TaskTracker.Web.Services.Folder.Dtos;

namespace TaskTracker.Web.Services.Tasks.Dtos;

public record TaskDto
{
    public Guid Id { get; set; }
    public required FolderDto Folder { get; set; }
    public required string Content { get; set; }
    public bool IsCompleted { get; set; }
}
