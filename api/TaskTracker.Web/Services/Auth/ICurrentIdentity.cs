using static TaskTracker.Common.AppConstant;

namespace TaskTracker.Web.Services.Auth;

public interface ICurrentIdentity
{
    Guid GetUserGuid();
    string GetUserId();
    string[] GetUserRoles();
}

public class CurrentIdentity : ICurrentIdentity
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentIdentity(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == Claims.Id)?.Value;
        return userId ?? throw new Exception("Unauthorized");
    }

    public Guid GetUserGuid()
    {
        return Guid.Parse(GetUserId());
    }

    public string[] GetUserRoles()
    {
        var roles = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == Claims.Roles)?.Value;
        return roles?.Split(',') ?? Array.Empty<string>();
    }
}
