namespace GeminiServer.Core;

public class RequestHandler<TRequest, TResponse>(IMiddlewareRegistry<State<TRequest, TResponse>> middlewareRegistry) : IRequestHandler<TRequest, TResponse>
{
    private readonly Invoke<State<TRequest, TResponse>> _middlewarePipeline = middlewareRegistry.Wire();
    public async Task<TResponse> HandleAsync(TRequest request)
    {
        var state = new State<TRequest, TResponse>(request);
        await _middlewarePipeline.Invoke(state);
        return state.Response ?? throw new InvalidOperationException("No response returned");
    }
}