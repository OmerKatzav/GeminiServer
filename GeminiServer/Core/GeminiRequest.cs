namespace GeminiServer.Core;

public class GeminiRequest
{
    public required string Path { get; init; }
    public string? QueryString { get; init; }
    public required GeminiRequestMetadata Metadata { get; init; }
}