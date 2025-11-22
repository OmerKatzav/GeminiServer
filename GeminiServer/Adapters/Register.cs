using GeminiServer.Abstractions;
using GeminiServer.Adapters.Presentation;
using GeminiServer.Adapters.Transport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GeminiServer.Adapters;

public static class Extensions
{
    public static IServiceCollection UseTls(this IServiceCollection collection)
    {
        return collection
            .AddHostedService<Tcp>()
            .AddSingleton<IPresentation, Tls>();
    }

    public static IHostApplicationBuilder UseTls(this IHostApplicationBuilder builder)
    {
        builder.Services.UseTls();
        return builder;
    }
}