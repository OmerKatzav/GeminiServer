namespace GeminiServer.Core;

public class RouterMiddleware<TState> : IMiddleware<TState>
{
    public Task Invoke(TState state, Invoke<TState> next)
    {
        throw new NotImplementedException();
    }
}