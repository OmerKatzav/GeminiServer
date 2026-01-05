namespace GeminiServer.Core;

public class ClientHandler<TRequest, TResponse>(IProtocolHandler<TRequest, TResponse> protocolHandler, IRequestHandler<TRequest, TResponse> requestHandler, IExceptionHandler<TResponse> exceptionHandler) : IClientHandler
{
    public async Task HandleAsync(Stream stream, IDictionary<string, object?> metadata, CancellationToken ct)
    {
        try
        {
            var request = await protocolHandler.GetRequestAsync(stream);
            var response = await requestHandler.HandleAsync(request, metadata);
            await protocolHandler.SendResponseAsync(stream, response);
        }
        catch (Exception ex)
        {
            var response = await exceptionHandler.HandleAsync(ex, metadata);
            await protocolHandler.SendResponseAsync(stream, response);
            throw;
        }
    }
}