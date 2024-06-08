using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Data;
using TaskTracker.Data.Entities;
using TaskTracker.Web;
using TaskTracker.Web.Extensions;
using TaskTracker.Web.Middlewares;
using TaskTracker.Web.Services.Auth;
using TaskTracker.Web.Services.Folder;
using TaskTracker.Web.Services.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TaskTrackerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

// Register IDbContext as TaskTrackerDbContext
builder.Services.AddScoped<IDbContext>(provider => provider.GetRequiredService<TaskTrackerDbContext>());

builder.Services.AddScoped<IFolderService, FolderService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ICurrentIdentity, CurrentIdentity>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy
        .WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
    );
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddIdentity<UserEntity, RoleEntity>()
    .AddEntityFrameworkStores<TaskTrackerDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
       // to ingore loop inside entities that reference each other
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.AddBearer(builder.Configuration["Jwt:Secret"] ?? throw new Exception("Jwt Secret does not exist"));
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwagger();

var app = builder.Build();

await app.MigrateDatabaseAsync();
await app.SeedData();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
