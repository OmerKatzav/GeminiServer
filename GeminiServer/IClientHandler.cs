namespace GeminiServer;

public interface IClientHandler
{
    Task HandleAsync(Stream stream, IDictionary<string, object?> metadata, CancellationToken ct);
}