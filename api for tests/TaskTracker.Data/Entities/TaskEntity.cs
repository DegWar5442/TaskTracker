namespace TaskTracker.Data.Entities;

public record TaskEntity : BaseEntity
{
    public required string Content { get; set; }

    public bool IsCompleted { get; set; }

    public Guid FolderId { get; set; }
    public required FolderEntity Folder { get; set; }
}