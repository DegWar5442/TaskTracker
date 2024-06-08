namespace TaskTracker.Web.Services.Tasks.Dtos;

public record UpdateTaskDto
{
    public Guid Id { get; set; }
    public required string Content { get; set; }
    public bool IsCompleted { get; set; }
    public Guid FolderId { get; set; }
}
