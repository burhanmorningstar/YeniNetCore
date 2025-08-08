using CleanApp.Api.ExceptionHandler;

namespace CleanApp.Api.Extensions
{
    public static class ExceptionHandlerExtension
    {
        public static IServiceCollection AddExceptionHandlerExt(this IServiceCollection services)
        {
            services.AddExceptionHandler<CriticalExceptionHandler>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            return services;
        }
    }
}
