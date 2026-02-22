using GeminiServer.Abstractions;

namespace GeminiServer.Core.Gemini;

public class GeminiBaseMiddleware : IMiddleware<State<GeminiRequest, GeminiResponse>>
{
    public Task InvokeAsync(State<GeminiRequest, GeminiResponse> state, InvokeAsync<State<GeminiRequest, GeminiResponse>> next)
    {
        state.Response ??= new GeminiResponse { StatusCode = GeminiStatusCodes.NotFound, Header = "Resource Not Found" };
        return Task.CompletedTask;
    }
}