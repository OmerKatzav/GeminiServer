namespace GeminiServer.Core.Gemini;

public class GeminiResponse
{
    public GeminiStatusCodes StatusCode { get; set; } = GeminiStatusCodes.Success;
    public string Header { get; set; } = string.Empty;
    public ReadOnlyMemory<byte>? Content { get; set; }
}