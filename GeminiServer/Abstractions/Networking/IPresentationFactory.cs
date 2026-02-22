namespace GeminiServer.Abstractions.Networking;

public interface IPresentationFactory
{
    public IPresentation Create(Stream stream, IDictionary<string, object?> metadata);
}