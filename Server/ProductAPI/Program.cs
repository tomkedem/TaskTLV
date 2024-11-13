using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.ConfigureApplicationServices(builder.Configuration);
builder.Services.ConfigureJwtAuthentication(builder.Configuration);
builder.Services.ConfigureCors(); // Add CORS configuration here
builder.Services.AddAuthorization();
builder.Services.AddControllers(); // Enables MVC controllers
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();

var app = builder.Build();

// Seed the database with initial data on startup
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await seeder.SeedDataAsync();
}

// Enable Swagger UI only in Development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API v1");
        c.RoutePrefix = "swagger"; // Sets the URL to /swagger/index.html
    });
}

app.UseCustomExceptionHandling();
app.UseHttpsRedirection();
app.UseCors("AllowAngularApp"); // Apply the CORS policy here

app.UseAuthentication();
app.UseAuthorization();
// Map controllers
app.MapControllers();
app.Run();
