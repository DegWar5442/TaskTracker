using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskTracker.Common;
using TaskTracker.Common.Exceptions;
using TaskTracker.Data.Entities;
using TaskTracker.Web.Services.Auth.Dtos;

namespace TaskTracker.Web.Services.Auth;

public interface IAuthService
{
    public Task<AuthResponse> LoginUser(LoginUserDto loginUserDto, CancellationToken cancellationToken);
    public Task<AuthResponse> RegisterUser(RegisterUserDto registerUserDto, CancellationToken cancellationToken);
    public Task<AuthResponse> Me(CancellationToken cancellationToken);
}

public class AuthService(
    IJwtService jwtService, 
    IMapper mapper, 
    UserManager<UserEntity> userManager, 
    ICurrentIdentity currentIdentity) : IAuthService
{
    public async Task<AuthResponse> RegisterUser(RegisterUserDto registerUserDto, CancellationToken cancellationToken)
    {
        var user = mapper.Map<UserEntity>(registerUserDto);

        var result = await userManager.CreateAsync(user, registerUserDto.Password);

        if (!result.Succeeded)
        {
            throw new BadRequestException("User with same user name already exists", result.Errors);
        }

        await userManager.AddToRoleAsync(user, AppConstant.Roles.User);

        var token = jwtService.GenerateToken(
            user.Id.ToString(),
            string.Join(", ", await userManager.GetRolesAsync(user)),
            AppConstant.JwtTokenLifetimes.Default);

        return new AuthResponse()
        {
            Token = token,
            User = mapper.Map<UserDto>(user)
        };
    }

    public async Task<AuthResponse> LoginUser(LoginUserDto loginUserDto, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(loginUserDto.UserName) ?? throw new BadRequestException("Invalid credentials");

        bool isPasswordValid = await userManager.CheckPasswordAsync(user, loginUserDto.Password);

        if (!isPasswordValid)
        {
            throw new BadRequestException("Invalid credentials");
        }

        TimeSpan tokenLifetime = AppConstant.JwtTokenLifetimes.Default;

        var token = jwtService.GenerateToken(
            user.Id.ToString(),
            string.Join(", ", await userManager.GetRolesAsync(user)),
            tokenLifetime);

        return new AuthResponse()
        {
            Token = token,
            User = mapper.Map<UserDto>(user)
        };
    }

    public async Task<AuthResponse> Me(CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(currentIdentity.GetUserId()) ?? throw new BadRequestException("User not found");

        return new AuthResponse()
        {
            Token = jwtService.GenerateToken(
                user.Id.ToString(),
                string.Join(", ", await userManager.GetRolesAsync(user)),
                AppConstant.JwtTokenLifetimes.Default),
            User = mapper.Map<UserDto>(user)
        };
    }
}
