using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Services;

namespace ProductAPI.Helpers
{
    public static class ServiceExtensions
    {
        public static void ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Database context setup
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Add other services
            services.AddScoped<AuthService>();
            services.AddScoped<ProductService>();

            // Add data seeding
            services.AddTransient<DataSeeder>();
        }
    }
}
