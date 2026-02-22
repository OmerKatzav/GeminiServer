namespace GeminiServer.Abstractions;

public delegate Task InvokeAsync<in TState>(TState state);

public interface IMiddleware<TState>
{
    Task InvokeAsync(TState state, InvokeAsync<TState> next);
}