using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GeminiServer.Config;

public static class Extensions
{
    public static IHostApplicationBuilder ConfigureNetwork(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<NetworkConfig>(builder.Configuration.GetSection("Network"));
        return builder;
    }
    
    public static IHostApplicationBuilder ConfigureStaticFiles(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<StaticFileConfig>(builder.Configuration.GetSection("StaticFile"));
        return builder;
    }
    
    public static IHostApplicationBuilder Configure(this IHostApplicationBuilder builder)
    {
        return builder
            .ConfigureNetwork()
            .ConfigureStaticFiles();
    }
}