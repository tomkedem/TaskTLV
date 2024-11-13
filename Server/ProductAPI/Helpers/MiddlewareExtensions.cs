using Microsoft.AspNetCore.Builder;

namespace ProductAPI.Helpers
{
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Adds custom error handling middleware to log unhandled exceptions.
        /// </summary>
        public static void UseCustomExceptionHandling(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception ex)
                {
                    // Log the exception (logging code goes here if necessary)
                    Console.WriteLine($"Unhandled exception: {ex.Message}");

                    // Set the response status and message
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("An unexpected error occurred.");
                }
            });
        }
    }
}
