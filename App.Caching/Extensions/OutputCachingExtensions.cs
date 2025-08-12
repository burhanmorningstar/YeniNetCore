using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Caching.Extensions;

public static class OutputCachingExtensions
{
    private const string PolicyName = "ApiDefault";

    public static IServiceCollection AddApiOutputCaching(this IServiceCollection services, IConfiguration config)
    {
        var s = config.GetSection("OutputCache").Get<OutputCacheSettings>() ?? new();

        if (!s.Enabled) return services;

        services.AddOutputCache(opt =>
        {
            opt.AddPolicy(PolicyName, b =>
            {
                b.Expire(TimeSpan.FromSeconds(s.ExpireSeconds));
                if (s.VaryByAllQuery) b.SetVaryByQuery("*");
                if (s.VaryByHeaders is { Length: > 0 }) b.SetVaryByHeader(s.VaryByHeaders);
            });
        });

        if (s.UseRedis)
        {
            services.AddStackExchangeRedisOutputCache(o =>
            {
                o.Configuration = config["Redis:Configuration"];
                o.InstanceName = config["Redis:InstanceName"];
            });
        }

        return services;
    }

    public static IApplicationBuilder UseApiOutputCaching(this IApplicationBuilder app, IConfiguration config)
    {
        var s = config.GetSection("OutputCache").Get<OutputCacheSettings>() ?? new();
        if (s.Enabled) app.UseOutputCache();
        return app;
    }

    public static IEndpointConventionBuilder MapControllersWithOutputCache(
        this IEndpointRouteBuilder app, IConfiguration config)
    {
        var s = config.GetSection("OutputCache").Get<OutputCacheSettings>() ?? new();
        var builder = app.MapControllers();

        if (!s.Enabled) return builder;
        builder.Add(endpointBuilder =>
        {
            endpointBuilder.Metadata.Add(new OutputCacheAttribute { PolicyName = PolicyName });
        });

        return builder;
    }
}
