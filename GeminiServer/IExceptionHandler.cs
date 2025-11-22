namespace GeminiServer;

public interface IExceptionHandler<in TRequestMetadata, TResponse>
{
    public Task<TResponse> HandleAsync(Exception ex, TRequestMetadata requestMetadata);
}