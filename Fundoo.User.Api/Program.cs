using Fundoo.User.Application.Interfaces;
using Fundoo.User.Application.UserApplication;
using Fundoo.User.Infrastructure.Data;
using Fundoo.User.Infrastructure.Repository;
using Fundoo.User.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Fundoo.User.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // 1. Database Context Connection (MySQL via Pomelo)
            var connectionString = builder.Configuration.GetConnectionString("UserDb");
            builder.Services.AddDbContext<UserDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // 2. Dependency Injection (DI) Wire-up for Clean Architecture
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUserApplication, UserApplication>();

            var app = builder.Build();

            // Health check endpoint
            app.MapGet("/health", () => Results.Ok(new { status = "User API is healthy", timestamp = DateTime.UtcNow }));

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}