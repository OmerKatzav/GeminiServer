namespace GeminiServer.Core;

public record State<TRequest, TResponse>(TRequest Request)
{
    public Dictionary<string, object?> Data { get; init; } = new();
    public TResponse? Response { get; set; }
}