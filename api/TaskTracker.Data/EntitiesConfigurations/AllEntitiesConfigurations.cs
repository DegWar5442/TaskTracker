using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskTracker.Data.Entities;

namespace TaskTracker.Data.EntitiesConfigurations;

internal class RoleEntityConfiguration : BaseEntityConfiguration<RoleEntity>
{
}

internal class UserEntityConfiguration : BaseEntityConfiguration<UserEntity>
{

}

internal class TaskEntityConfiguration : BaseEntityConfiguration<TaskEntity>
{
    public override void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        base.Configure(builder);

        builder.HasOne(t => t.Folder)
            .WithMany(f => f.Tasks)
            .HasForeignKey(t => t.FolderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

internal class FolderEntityConfiguration : BaseEntityConfiguration<FolderEntity>
{
}