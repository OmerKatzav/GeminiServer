using System.Diagnostics.CodeAnalysis;

namespace GeminiServer;

public interface IRouterService<TState>
{
    public void RegisterMiddleware<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TMiddleware>(string path);
    public void RegisterMiddleware(IMiddleware<TState> middleware, string path);
    public Invoke<TState> Wire(string path, Invoke<TState> next);
} 