namespace GeminiServer.Abstractions;

public interface IPresentationFactory
{
    public IPresentation Create(Stream stream, IDictionary<string, object?> metadata);
}