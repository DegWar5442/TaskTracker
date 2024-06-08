namespace TaskTracker.Web.Services.Auth.Dtos;

public record UserDto
{
    public Guid Id { get; set; }
    public required string UserName { get; set; }
}