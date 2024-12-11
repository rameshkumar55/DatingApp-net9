using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration configuration) : ITokenService
{
    public string GenerateJwtToken(AppUser appUser)
    {
        var tokenKey = configuration["TokenKey"] ?? throw new Exception("Can not access token key from appsetting file");
        if (tokenKey.Length < 64) throw new Exception("Token key should be longer");

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, appUser.UserName),
            //new Claim(ClaimTypes.Email, appUser.Email),
            //new Claim(ClaimTypes.Role, appUser.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "dating-app",
            audience: "dating-app-users",
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }   
    
}
