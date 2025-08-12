using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Messaging.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessagingExt(this IServiceCollection services, IConfiguration _)
    {
        return services;
    }
}