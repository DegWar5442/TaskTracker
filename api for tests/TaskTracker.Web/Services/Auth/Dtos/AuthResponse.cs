namespace TaskTracker.Web.Services.Auth.Dtos;

public record AuthResponse
{
    public required string Token { get; set; }
    public required UserDto User { get; set; }
};