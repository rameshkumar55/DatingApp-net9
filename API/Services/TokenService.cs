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
    public string CreateToekn(AppUser appUser)
    {
        var tokenKey = configuration["TokenKey"] ?? throw new Exception("Can not access token key from appsetting file");
        if (tokenKey.Length < 64) throw new Exception("Token key should be longer");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        var claims = new List<Claim>{
            new(ClaimTypes.NameIdentifier,appUser.UserName)
        };

        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = cred
        };

        var tokenHendler = new JwtSecurityTokenHandler();
        var token = tokenHendler.CreateToken(tokenDescriptor);
        return tokenHendler.WriteToken(token);
    }
}
