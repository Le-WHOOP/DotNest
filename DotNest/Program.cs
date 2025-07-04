using DotNest.DataAccess.Entities;
using DotNest.DataAccess.Interfaces;
using DotNest.DataAccess.Repositories;
using DotNest.Services;
using DotNest.Services.Interfaces;
using DotNest.Services.Mapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace DotNest;

public static class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddAutoMapper(typeof(AutomapperProfiles));

        builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

        builder.Services.AddDbContext<DotNestContext>(options =>
        {
            options.UseLazyLoadingProxies().UseSqlServer("Name=ConnectionStrings:SqlServer");
        });

        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        // repositories
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IRentalRepository, RentalRepository>();
        builder.Services.AddScoped<IPictureRepository, PictureRepository>();
        builder.Services.AddScoped<IBookingRepository, BookingRepository>();

        // services
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IRentalService, RentalService>();
        builder.Services.AddScoped<ILocationService, LocationService>();
        builder.Services.AddScoped<IBookingService, BookingService>();


        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/StatusCode/500");
            app.UseStatusCodePagesWithReExecute("/StatusCode/{0}");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        // for status code
        app.UseStatusCodePagesWithReExecute("/StatusCode/{0}");
        app.UseExceptionHandler("/StatusCode/500");

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseStaticFiles();

        app.UseAuthorization();

        app.MapStaticAssets();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Location}/{action=Index}")
            .WithStaticAssets();
       

        app.Run();
    }
}
