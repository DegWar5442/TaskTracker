namespace TaskTracker.Data.Entities;

public interface IEntity
{
    Guid Id { get; set; }

    DateTime CreatedAt { get; set; }
    DateTime ModifiedAt { get; set; }
}

public abstract record BaseEntity : IEntity
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}