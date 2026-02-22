using System.Net.Security;
using GeminiServer.Abstractions.Networking;

namespace GeminiServer.Adapters.Networking;

public class Tls(SslStream stream) : IPresentation
{
    public Stream Stream => stream;

    public async Task StopAsync()
    {
        await stream.ShutdownAsync();
    }
}