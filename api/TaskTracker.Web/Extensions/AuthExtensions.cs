using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TaskTracker.Web.Extensions;

public static partial class AuthExtensions
{
    public static IServiceCollection AddBearer(this IServiceCollection services, string jwtSecret)
    {
        byte[] secret = Encoding.ASCII.GetBytes(jwtSecret);
        var signingKey = new SymmetricSecurityKey(secret);

        services.AddTransient(provider => signingKey);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = signingKey,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        return services;
    }
}
