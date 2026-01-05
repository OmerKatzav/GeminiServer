using GeminiServer.Core;

namespace GeminiServer.Gemini;

public class GeminiBaseMiddleware : IMiddleware<State<GeminiRequest, GeminiResponse>>
{
    public Task Invoke(State<GeminiRequest, GeminiResponse> state, Invoke<State<GeminiRequest, GeminiResponse>> next)
    {
        state.Response ??= new GeminiResponse { StatusCode = GeminiStatusCodes.NotFound, Header = "Resource Not Found" };
        return Task.CompletedTask;
    }
}