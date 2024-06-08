using Microsoft.AspNetCore.Identity;

namespace TaskTracker.Data.Entities;

public class UserEntity : IdentityUser<Guid>, IEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }

    public List<FolderEntity> Folders { get; set; } = [];
}