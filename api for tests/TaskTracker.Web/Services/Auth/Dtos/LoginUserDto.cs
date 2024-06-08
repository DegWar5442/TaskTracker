namespace TaskTracker.Web.Services.Auth.Dtos;

public record LoginUserDto
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}

public record RegisterUserDto
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}