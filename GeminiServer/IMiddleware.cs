namespace GeminiServer;

public delegate Task Invoke<in TState>(TState state);

public interface IMiddleware<TState>
{
    Task Invoke(TState state, Invoke<TState> next);
}

public static class MiddlewareExtensions
{
    public static Invoke<TState> Chain<TState>(this IMiddleware<TState> middleware, Invoke<TState> next)
    {
        return state => middleware.Invoke(state, next);
    }
}