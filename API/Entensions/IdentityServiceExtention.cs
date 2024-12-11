using System;
using System.Text;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Entensions;

public static class IdentityServiceExtention
{
    public static IServiceCollection AddIdentityService(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
                {
                    var tokenKey = configuration["TokenKey"] ?? throw new Exception("Can not access token key from appsetting file");
                    if (tokenKey.Length < 64) throw new Exception("Token key should be longer");
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey))
                    };

                });
        service.AddScoped<ITokenService, TokenService>();
        return service;
    }
}
