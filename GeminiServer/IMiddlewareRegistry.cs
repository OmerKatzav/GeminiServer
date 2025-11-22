namespace GeminiServer;

public interface IMiddlewareRegistry<TState>
{
    public void RegisterMiddleware<TMiddleware>() where TMiddleware : IMiddleware<TState>;
    public Invoke<TState> Wire();
}