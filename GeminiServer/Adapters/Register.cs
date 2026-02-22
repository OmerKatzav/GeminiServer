using GeminiServer.Abstractions.Networking;
using GeminiServer.Adapters.Networking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GeminiServer.Adapters;

public static class Extensions
{
    public static IServiceCollection UseTls(this IServiceCollection collection)
    {
        return collection
            .AddHostedService<Tcp>()
            .AddSingleton<IPresentationFactory, TlsFactory>();
    }

    public static IHostApplicationBuilder UseTls(this IHostApplicationBuilder builder)
    {
        builder.Services.UseTls();
        return builder;
    }
}