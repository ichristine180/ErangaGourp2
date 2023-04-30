using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using E_ranga.Data;

using dotenv.net;
using DotNetEnv;
using Namespace;
using Namespace2;

namespace E_ranga{
    public static class Program{
        public static void Main(string[] args)
        {
            DotEnv.Load();
            DotNetEnv.Env.Load();
            var builder = WebApplication.CreateBuilder(args);
            // Get the configuration
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            // Replace environment variable placeholders in configuration
            configuration["ConnectionStrings:DefaultConnection"] = configuration["ConnectionStrings:DefaultConnection"]
                .Replace("${DB_HOST}", Environment.GetEnvironmentVariable("DB_HOST"))
                .Replace("${DB_PORT}", Environment.GetEnvironmentVariable("DB_PORT"))
                .Replace("${DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME"))
                .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER"))
                .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD"));

            //var connectionString = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};Port={Environment.GetEnvironmentVariable("DB_PORT")};Database={Environment.GetEnvironmentVariable("DB_NAME")};Username={Environment.GetEnvironmentVariable("DB_USER")};Password={Environment.GetEnvironmentVariable("DB_PASSWORD")}";
            
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            var app = builder.Build();
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "register",
    pattern: "register",
    defaults: new { controller = "User", action = "Register" });

app.MapControllerRoute(
    name: "login",
    pattern: "login",
    defaults: new { controller = "User", action = "Login" });

app.MapControllerRoute(
    name: "dashboard",
    pattern: "dashboard",
    defaults: new { controller = "User", action = "Dashboard" });
            app.Run();
        }
    }
}
