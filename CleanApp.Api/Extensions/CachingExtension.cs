using App.Caching;

namespace App.Api.Extensions
{
    public static class CachingExtension
    {
        public static IServiceCollection AddCachingExt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddCaching(configuration);
            return services;
        }
    }
}