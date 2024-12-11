using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;


namespace API.Entensions;

public static class ApplicationServiceExtension
{
    public static IServiceCollection AddApplicationService(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddControllers();
        service.AddDbContext<DataContext>(option =>
        {
            option.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });
        service.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", policy =>
            {
                policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });
        service.AddScoped<ITokenService, TokenService>();

        return service;
    }
}
