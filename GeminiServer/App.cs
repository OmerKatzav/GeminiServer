using System.Diagnostics.CodeAnalysis;
using GeminiServer.Adapters;
using GeminiServer.Config;
using GeminiServer.Core;
using GeminiServer.Gemini;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GeminiServer;


public static class GeminiApp
{
    public static HostApplicationBuilder CreateApplicationBuilder()
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Configure();
        builder.UseTls();
        builder.Services.AddSingleton<IMiddlewareRegistry<State<GeminiRequest, GeminiResponse>>, MiddlewareRegistry<State<GeminiRequest, GeminiResponse>, GeminiBaseMiddleware>>();
        builder.Services.AddSingleton<IClientHandler, ClientHandler<GeminiRequest, GeminiResponse>>();
        builder.Services.AddSingleton<IExceptionHandler<GeminiResponse>, GeminiExceptionHandler>();
        builder.Services.AddSingleton<IProtocolHandler<GeminiRequest, GeminiResponse>, GeminiProtocolHandler >();
        builder.Services.AddSingleton<IRequestHandler<GeminiRequest, GeminiResponse>, RequestHandler<GeminiRequest, GeminiResponse>>();
        return builder;
    }

    public static IHost UseMiddleware<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TMiddleware>(this IHost host) where TMiddleware : IMiddleware<State<GeminiRequest, GeminiResponse>>
    {
        host.Services.GetRequiredService<IMiddlewareRegistry<State<GeminiRequest, GeminiResponse>>>().RegisterMiddleware<TMiddleware>();
        return host;
    }
}