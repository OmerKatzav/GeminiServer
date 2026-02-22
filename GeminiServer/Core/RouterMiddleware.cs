using GeminiServer.Abstractions;

namespace GeminiServer.Core;

public class RouterMiddleware<TState> : IMiddleware<TState>
{
    public Task InvokeAsync(TState state, InvokeAsync<TState> next)
    {
        throw new NotImplementedException();
    }
}