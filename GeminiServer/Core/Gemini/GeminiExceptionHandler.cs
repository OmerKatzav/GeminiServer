using GeminiServer.Abstractions;

namespace GeminiServer.Core.Gemini;

public class GeminiExceptionHandler : IExceptionHandler<GeminiResponse>
{
    public Task<GeminiResponse> HandleAsync(Exception ex, IDictionary<string, object?> requestMetadata)
    {
        var statusCode = GeminiStatusCodes.TempFailure;
        var header = string.Empty;
        if (ex is GeminiException geminiEx)
        {
            statusCode = geminiEx.StatusCode;
            header = geminiEx.ErrorMessage;
        }

        if (statusCode == GeminiStatusCodes.Success) statusCode = GeminiStatusCodes.TempFailure;

        return Task.FromResult(new GeminiResponse
        {
            StatusCode = statusCode,
            Header = header,
        });
    }
}