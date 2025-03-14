using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController(DataContext dataContext, ITokenService tokenService) : BaseApiController
    {
        [HttpPost("register")] //account//register
        public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.UserName))
            {
                return BadRequest("Username is already exists");
            }

            if (await EmailExists(registerDto.Email))
            {
                return BadRequest("Email is already exists");
            }

            if (await MobileNumberExists(registerDto.MobileNumber))
            {
                return BadRequest("MobileNumber is already exists");
            }
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = registerDto.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key,
                Email = registerDto.Email,
                MobileNumber = registerDto.MobileNumber
            };

            dataContext.Users.Add(user);
            await dataContext.SaveChangesAsync();
            return user;
        }

        [HttpPost("login")] //account//register
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {

            var user = await dataContext.Users.FirstOrDefaultAsync(x =>
            x.UserName.ToLower() == loginDto.UserName.ToLower().Trim() || 
            x.Email.ToLower() == loginDto.UserName.ToLower().Trim() || 
            x.MobileNumber.ToLower() == loginDto.UserName.ToLower().Trim());

            if (user == null)
            {
                return Unauthorized("Invalid username");
            }
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < ComputeHash.Length; i++)
            {
                if(ComputeHash[i]!=user.PasswordHash[i]){
                    return Unauthorized("Invalid password");
                }
            }           
            return new UserDto{
                Username=user.UserName,
                Token=tokenService.GenerateJwtToken(user)
            };
        }

        private async Task<bool> UserExists(string usename)
        {
            return await dataContext.Users.AnyAsync(u => u.UserName.ToLower() == usename.ToLower().Trim());
        }
         private async Task<bool> EmailExists(string email)
        {
            return await dataContext.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower().Trim());
        }

         private async Task<bool> MobileNumberExists(string mobile)
        {
            return await dataContext.Users.AnyAsync(u => u.MobileNumber.ToLower() == mobile.ToLower().Trim());
        }
    }
}
