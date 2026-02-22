using GeminiServer.Abstractions;

namespace GeminiServer.Core;

public class RequestHandler<TRequest, TResponse>(IMiddlewareRegistry<State<TRequest, TResponse>> middlewareRegistry) : IRequestHandler<TRequest, TResponse>
{
    public async Task<TResponse> HandleAsync(TRequest request, IDictionary<string, object?> metadata)
    {
        var state = new State<TRequest, TResponse>(request, metadata);
        await middlewareRegistry.InvokeAsync(state);
        return state.Response ?? throw new InvalidOperationException("No response returned from middlewares");
    }
}