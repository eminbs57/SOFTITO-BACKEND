using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using MvcProject.Services;
using MvcProject.Models;
using QuestPDF.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();

builder.Services.AddHttpClient<ApiService<RoomType>>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5055/api/RoomTypes/");
});
builder.Services.AddHttpClient<ApiService<Room>>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5055/api/Rooms/");
});
builder.Services.AddHttpClient<ApiService<Guest>>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5055/api/Guests/");
});
builder.Services.AddHttpClient<ApiService<Reservation>>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5055/api/Reservations/");
});

builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
    });

var app = builder.Build();

QuestPDF.Settings.License = LicenseType.Community;

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
