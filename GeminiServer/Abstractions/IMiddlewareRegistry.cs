using System.Diagnostics.CodeAnalysis;

namespace GeminiServer.Abstractions;

public interface IMiddlewareRegistry<TState>
{
    public void RegisterMiddleware<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TMiddleware>(Type[]? argTypes = null, object?[]? args = null) where TMiddleware : IMiddleware<TState>;
    public void RegisterMiddleware(Type middlewareType, Type[]? argTypes=null, object?[]? args=null);
    public Task InvokeAsync(TState state);
}