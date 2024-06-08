namespace TaskTracker.Data.Entities;

public record FolderEntity : BaseEntity
{
    public required string Name { get; set; }

    public Guid OwnerId { get; set; }
    public required UserEntity Owner { get; set; }

    public List<TaskEntity> Tasks { get; set; } = [];
}
