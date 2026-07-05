using Microsoft.EntityFrameworkCore;
using dbfirstProjem.Models;
using Microsoft.AspNetCore.Identity;
using QuestPDF.Infrastructure;
using Serilog;

QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// 1. SERVİSLERİ BURADA EKLE
builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=sinema;Trusted_Connection=True;TrustServerCertificate=True;"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});
// 2. BUILD İŞLEMİ
var app = builder.Build();

app.UseSerilogRequestLogging();

// 3. MIDDLEWARE SIRALAMASI
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    try { await next(); }
    catch (dbfirstProjem.Exceptions.BiletException ex)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("Hata: " + ex.Message);
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles(); // .MapStaticAssets() yerine genelde bu kullanılır
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();