using App.Api.Extensions;
using App.Application.Extensions;
using App.Bus;
using App.Caching.Extensions;
using App.Messaging.Extensions;
using App.Persistence.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllersWithFiltersExt()
    .AddSwaggerGenExt()
    .AddExceptionHandlerExt()
    .AddCachingExt(builder.Configuration)
    .AddApiOutputCaching(builder.Configuration)
    .AddRepositories(builder.Configuration)
    .AddServices(builder.Configuration)
    .AddMessagingExt(builder.Configuration)
    .AddBusExt(builder.Configuration);

var app = builder.Build();

app.UseConfigurePipelineExt();

app.UseApiOutputCaching(builder.Configuration);

app.MapControllersWithOutputCache(builder.Configuration);

await app.RunAsync();