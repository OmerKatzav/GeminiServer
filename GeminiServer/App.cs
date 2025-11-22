using GeminiServer.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GeminiServer;

public static class GeminiApp
{
    public static HostApplicationBuilder CreateApplicationBuilder()
    {
        var builder = Host.CreateApplicationBuilder();
        
        builder.Services.AddSingleton<IMiddlewareRegistry<State<GeminiRequest, GeminiResponse>>, MiddlewareRegistry<State<GeminiRequest, GeminiResponse>>>();
        return builder;
    }

    public static IHost UseMiddleware<TMiddleware>(this IHost host) where TMiddleware : IMiddleware<State<GeminiRequest, GeminiResponse>>
    {
        host.Services.GetRequiredService<MiddlewareRegistry<State<GeminiRequest, GeminiResponse>>>().RegisterMiddleware<TMiddleware>();
        return host;
    }
}