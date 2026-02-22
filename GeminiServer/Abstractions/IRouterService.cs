using System.Diagnostics.CodeAnalysis;

namespace GeminiServer.Abstractions;

public interface IRouterService<TState>
{
    public void RegisterMiddleware<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TMiddleware>(string path, Type[]? argTypes = null, object?[]? args = null);
    public void RegisterMiddleware(Type middlewareType, string path, Type[]? argTypes = null, object?[]? args = null);
    public InvokeAsync<TState> Wire(string path, InvokeAsync<TState> next);
} 