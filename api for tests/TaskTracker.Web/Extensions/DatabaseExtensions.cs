using Microsoft.EntityFrameworkCore;
using TaskTracker.Data;

namespace TaskTracker.Web.Extensions;

public static class DatabaseExtensions
{
    public static async Task MigrateDatabaseAsync(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();

        var services = serviceScope.ServiceProvider;
        var dbContext = services.GetRequiredService<TaskTrackerDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}