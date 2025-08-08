using CleanApp.Api.Filters;

namespace CleanApp.Api.Extensions
{
    public static class ControllerExtension
    {
        public static IServiceCollection AddControllersWithFiltersExt(this IServiceCollection services)
        {
            services.AddScoped(typeof(NotFoundFilter<,>));

            services.AddControllers(options =>
            {
                options.Filters.Add<FluentValidationFilter>();
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });

            return services;
        }
    }
}
