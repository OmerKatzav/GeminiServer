namespace GeminiServer.Core;

public class GeminiClientHandler(IProtocolHandler<GeminiRequest, GeminiResponse, GeminiRequestMetadata> protocolHandler, IRequestHandler<GeminiRequest, GeminiResponse> requestHandler, IExceptionHandler<GeminiRequestMetadata, GeminiResponse> exceptionHandler) : IClientHandler<GeminiRequestMetadata>
{
    public async Task HandleAsync(Stream stream, GeminiRequestMetadata requestMetadata, CancellationToken ct)
    {
        try
        {
            var request = await protocolHandler.GetRequestAsync(stream, requestMetadata);
            var response = await requestHandler.HandleAsync(request);
            await protocolHandler.SendResponseAsync(stream, response);
        }
        catch (Exception ex)
        {
            var response = await exceptionHandler.HandleAsync(ex, requestMetadata);
            await protocolHandler.SendResponseAsync(stream, response);
            throw;
        }
    }
}