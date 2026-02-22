using GeminiServer.Abstractions;

namespace GeminiServer.Core.Extensions;

public static class MiddlewareExtensions
{
    public static InvokeAsync<TState> Chain<TState>(this IMiddleware<TState> middleware, InvokeAsync<TState> next)
    {
        return state => middleware.InvokeAsync(state, next);
    }
}