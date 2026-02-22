namespace GeminiServer.Abstractions;

public interface IProtocolHandler<TRequest, in TResponse>
{
    Task<TRequest> GetRequestAsync(Stream stream);
    Task SendResponseAsync(Stream stream, TResponse response);
}