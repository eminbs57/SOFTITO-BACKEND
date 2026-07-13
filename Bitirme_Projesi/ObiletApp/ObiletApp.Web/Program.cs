using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ObiletApp.Core.Entities;
using ObiletApp.Core.Interfaces;
using ObiletApp.Infrastructure.Contexts;
using ObiletApp.Infrastructure.Repositories;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DbContext Configuration
builder.Services.AddDbContext<ObiletDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=localhost,1433;Database=ObiletDb;User Id=sa;Password=GucluSifre123!;TrustServerCertificate=True;MultipleActiveResultSets=true;"));

// Identity Configuration (Cookie Auth for MVC)
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false; // Kolay test için kuralları esnetelim
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<ObiletDbContext>()
.AddDefaultTokenProviders();

// Cookie Settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(3);
});

// Dependency Injection for UoW and Generic Repositories (For Dynamic Admin)
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IQrCodeService, QrCodeService>();

// Configure HttpClient to talk to the ObiletApp.API (EF API for Writes)
builder.Services.AddHttpClient("EfApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5108/api/"); // EF API HTTP port
});

// Configure HttpClient to talk to the ObiletApp.API.Dapper (Dapper API for Reads)
builder.Services.AddHttpClient("DapperApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5028/api/"); // Dapper API HTTP port
});

var app = builder.Build();

// Veritabanında Rolleri Oluşturan Seed (Başlangıç) Metodu
using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ObiletDbContext>();
        dbContext.Database.EnsureCreated();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var roles = new[] { "Admin", "User" };
        foreach (var role in roles)
        {
            if (!roleManager.RoleExistsAsync(role).Result)
            {
                roleManager.CreateAsync(new IdentityRole(role)).Wait();
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Veritabanı bağlantı hatası: {ex.Message}. SQL Server kapalı olabilir, UI açılmaya devam ediliyor...");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// Auth Middleware MUST be before MapStaticAssets and MapControllerRoute
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
