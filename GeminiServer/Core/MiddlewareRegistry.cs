using Microsoft.Extensions.DependencyInjection;

namespace GeminiServer.Core;

public class MiddlewareRegistry<TState, TBaseMiddleware>(IServiceProvider serviceProvider) : IMiddlewareRegistry<TState> where TBaseMiddleware : IMiddleware<TState>
{
    private readonly List<IMiddleware<TState>> _middlewares = [];
    
    public void RegisterMiddleware<TMiddleware>() where TMiddleware : IMiddleware<TState>
    {
        _middlewares.Add(ActivatorUtilities.CreateInstance<TMiddleware>(serviceProvider));
    }

    public Invoke<TState> Wire()
    {
        RegisterMiddleware<TBaseMiddleware>();
        Invoke<TState> currentFunc = _ => Task.CompletedTask;
        for (var i = _middlewares.Count - 1; i >= 0; i--)
        {
            currentFunc = _middlewares[i].Chain(currentFunc);
        }
        return currentFunc;
    }
}
