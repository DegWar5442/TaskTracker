using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskTracker.Data.Entities;

namespace TaskTracker.Data.EntitiesConfigurations;
internal class BaseEntityConfiguration<TEntity> 
    : IEntityTypeConfiguration<TEntity> where TEntity : class, IEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(k => k.Id);

        builder.Property(u => u.CreatedAt).HasDefaultValueSql("getutcdate()").ValueGeneratedOnAdd();
        builder.Property(u => u.ModifiedAt).HasDefaultValueSql("getutcdate()").ValueGeneratedOnAddOrUpdate();
    }
}
