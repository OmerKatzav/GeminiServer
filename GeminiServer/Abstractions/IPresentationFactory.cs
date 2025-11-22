namespace GeminiServer.Abstractions;

public interface IPresentationFactory<in TMetadata>
{
    public IPresentation CreateAndUpdateMetadata(Stream stream, TMetadata metadata);
}