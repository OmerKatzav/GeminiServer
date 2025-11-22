using System.Net.Security;
using GeminiServer.Abstractions;

namespace GeminiServer.Adapters.Presentation;

public class Tls(SslStream stream) : IPresentation
{
    public Stream Stream => stream;

    public async Task StopAsync()
    {
        await stream.ShutdownAsync();
    }
}