namespace GeminiServer.Core.Gemini;

public class GeminiRequest
{
    public required string Path { get; init; }
    public string? QueryString { get; init; }
}