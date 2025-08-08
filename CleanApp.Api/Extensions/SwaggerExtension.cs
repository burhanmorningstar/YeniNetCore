using Microsoft.OpenApi.Models;

namespace CleanApp.Api.Extensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwaggerGenExt(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo() { Title = "CleanApp API", Version = "v1" });
            });
            return services;
        }

        public static IApplicationBuilder UseSwaggerExt(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }
    }
}
