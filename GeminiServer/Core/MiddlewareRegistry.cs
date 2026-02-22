using System.Diagnostics.CodeAnalysis;
using GeminiServer.Abstractions;
using GeminiServer.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace GeminiServer.Core;

public class MiddlewareRegistry<TState,[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TBaseMiddleware>(IServiceProvider serviceProvider) : IMiddlewareRegistry<TState> where TBaseMiddleware : IMiddleware<TState>
{
    private readonly List<Func<IServiceProvider, IMiddleware<TState>>> _middlewareFactories = [];

    public void RegisterMiddleware<TMiddleware>(Type[]? argTypes = null, object?[]? args = null) where TMiddleware : IMiddleware<TState>
    {
        var factory = ActivatorUtilities.CreateFactory<TMiddleware>(argTypes ?? []);
        _middlewareFactories.Add(provider => factory(provider, args));
    }

    public void RegisterMiddleware(Type middlewareType, Type[]? argTypes=null, object?[]? args=null)
    {
        if (!middlewareType.IsAssignableTo(typeof(IMiddleware<TState>)))
            throw new InvalidOperationException($"{middlewareType.FullName} must implement {typeof(IMiddleware<TState>).FullName}");
        var factory = ActivatorUtilities.CreateFactory(middlewareType, argTypes ?? []);
        _middlewareFactories.Add(provider => (IMiddleware<TState>)factory(provider, args));
    }

    public async Task InvokeAsync(TState state)
    {
        RegisterMiddleware<TBaseMiddleware>();
        
        await using var scope = serviceProvider.CreateAsyncScope();
        InvokeAsync<TState> currentFunc = _ => Task.CompletedTask;
        for (var i = _middlewareFactories.Count - 1; i >= 0; i--)
        {
            currentFunc = _middlewareFactories[i](scope.ServiceProvider).Chain(currentFunc);
        }
        
        await currentFunc(state);
    }
}
