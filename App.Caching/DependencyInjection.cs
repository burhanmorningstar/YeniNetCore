using App.Application.Contracts.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;



namespace App.Caching;

public static class DependencyInjection
{
    public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration config)
    {
        var section = config.GetSection("Redis");
        services.AddStackExchangeRedisCache(o =>
        {
            o.Configuration = section["Configuration"];
            o.InstanceName = section["InstanceName"];
        });

        services.AddSingleton<ICacheService, RedisCacheService>();
        return services;
    }
}