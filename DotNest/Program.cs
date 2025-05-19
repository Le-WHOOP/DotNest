using DotNest.DataAccess.Interfaces;
using DotNest.DataAccess.Repositories;
using DotNest.Services;
using DotNest.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DotNest;

public static class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

        //builder.Services.AddDbContext<PostgresContext>(options =>
        //{
        //    options.UseSqlServer("Name=ConnectionStrings:SqlServer");
        //});

        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        // repositories
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        // services
        builder.Services.AddScoped<IUserService, UserService>();


        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=User}/{action=Login}/{id?}")
            .WithStaticAssets();
       

        app.Run();
    }
}
