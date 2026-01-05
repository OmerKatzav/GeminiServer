using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace GeminiServer.Core;

public class MiddlewareRegistry<TState,[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TBaseMiddleware>(IServiceProvider serviceProvider) : IMiddlewareRegistry<TState> where TBaseMiddleware : IMiddleware<TState>
{
    private readonly List<IMiddleware<TState>> _middlewares = [];
    
    public void RegisterMiddleware<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TMiddleware>() where TMiddleware : IMiddleware<TState>
    {
        RegisterMiddleware(ActivatorUtilities.CreateInstance<TMiddleware>(serviceProvider));
    }

    public void RegisterMiddleware(IMiddleware<TState> middleware)
    {
        _middlewares.Add(middleware);
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
