namespace GeminiServer.Abstractions;

public interface IPresentation
{
    public Stream Stream { get; }
    public Task StopAsync();
}