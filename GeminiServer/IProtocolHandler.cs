namespace GeminiServer;

public interface IProtocolHandler<TRequest, in TResponse, in TRequestMetadata>
{
    Task<TRequest> GetRequestAsync(Stream stream, TRequestMetadata requestMetadata);
    Task SendResponseAsync(Stream stream, TResponse response);
}