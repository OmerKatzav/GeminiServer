using System.Diagnostics.CodeAnalysis;

namespace GeminiServer;

public interface IMiddlewareRegistry<TState>
{
    public void RegisterMiddleware<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TMiddleware>() where TMiddleware : IMiddleware<TState>;
    public void RegisterMiddleware(IMiddleware<TState> middleware);
    public Invoke<TState> Wire();
}