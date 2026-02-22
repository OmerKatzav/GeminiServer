namespace GeminiServer.Abstractions;

public interface IExceptionHandler<TResponse>
{
    public Task<TResponse> HandleAsync(Exception ex, IDictionary<string, object?> metadata);
}