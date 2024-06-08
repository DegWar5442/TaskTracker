namespace TaskTracker.Common;

public static class AppConstant
{
    public record Roles
    {
        public const string User = "User";
        public const string Admin = "Admin";

        public static readonly string[] All = [User, Admin];
    }

    public record Claims
    {
        public const string Id = "id";
        public const string Roles = "roles";
    }

    public record JwtTokenLifetimes
    {
        public static readonly TimeSpan Default = TimeSpan.FromHours(12);
    }

    public record ExceptionMessages
    {
        public const string NotOwner = "Unauthorized";
    }
}
