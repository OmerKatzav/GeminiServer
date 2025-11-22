namespace GeminiServer;

public interface IClientHandler<in TRequestMetadata>
{
    Task HandleAsync(Stream stream, TRequestMetadata requestMetadata, CancellationToken ct);
}