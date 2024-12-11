using System.Text;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthentication();
app.MapControllers();

app.Run();
