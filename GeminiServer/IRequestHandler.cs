namespace GeminiServer;

public interface IRequestHandler<in TRequest, TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, IDictionary<string, object?> metadata);
}