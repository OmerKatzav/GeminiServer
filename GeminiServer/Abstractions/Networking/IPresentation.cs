namespace GeminiServer.Abstractions.Networking;

public interface IPresentation
{
    public Stream Stream { get; }
    public Task StopAsync();
}