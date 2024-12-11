using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Entensions;

public static class IdentityServiceExtention
{
    public static IServiceCollection AddIdentityService(this IServiceCollection service, IConfiguration configuration)
    {        
        service.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var tokenKey = configuration["TokenKey"] ?? throw new Exception("Can not access token key from appsetting file");
            if (tokenKey.Length < 64) throw new Exception("Token key should be longer");
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "dating-app",
                ValidAudience = "dating-app-users",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey))
            };
        });
        
        return service;
    }
}
