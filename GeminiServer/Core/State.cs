namespace GeminiServer.Core;

public record State<TRequest, TResponse>(TRequest Request, IDictionary<string, object?> Metadata)
{
    public TResponse? Response { get; set; }
}