using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Web.Services.Auth;
using TaskTracker.Web.Services.Auth.Dtos;

namespace TaskTracker.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<AuthResponse> Register([FromBody] RegisterUserDto registerUserDto, CancellationToken cancellationToken)
    {
        return await authService.RegisterUser(registerUserDto, cancellationToken);
    }

    [HttpPost("login")]
    public async Task<AuthResponse> Login([FromBody] LoginUserDto loginUserDto, CancellationToken cancellationToken)
    {
        return await authService.LoginUser(loginUserDto, cancellationToken);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<AuthResponse> Me(CancellationToken cancellationToken)
    {
        return await authService.Me(cancellationToken);
    }
}
