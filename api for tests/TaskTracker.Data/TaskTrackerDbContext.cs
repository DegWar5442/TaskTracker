using System.Reflection;
using TaskTracker.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TaskTracker.Data;

public interface IDbContext
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    public DbSet<T> Set<T>()
        where T : class;
}

public class TaskTrackerDbContext : IdentityDbContext<UserEntity, RoleEntity, Guid>, IDbContext
{
    public DbSet<TaskEntity> Tasks { get; set; }
    public DbSet<FolderEntity> Folders { get; set; }

    public TaskTrackerDbContext(DbContextOptions<TaskTrackerDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}