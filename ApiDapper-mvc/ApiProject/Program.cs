var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var app = builder.Build();

try
{
    ApiProject.Models.Context.InitializeDatabase();
}
catch (System.Exception ex)
{
    System.Console.WriteLine("Veritabanı başlatılamadı: " + ex.Message);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Otel API v1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at the app's root (http://localhost:5055/)
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
