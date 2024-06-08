namespace TaskTracker.Web.Services.Tasks.Dtos;

public record CreateTaskDto
{
    public required string Content { get; set; }
    public bool IsCompleted { get; set; }
    public Guid FolderId { get; set; }
}
