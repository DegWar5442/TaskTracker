using Microsoft.AspNetCore.Identity;

namespace TaskTracker.Data.Entities;

public class RoleEntity : IdentityRole<Guid>, IEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}
