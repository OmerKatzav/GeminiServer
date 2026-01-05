using System.Text;
using GeminiServer.Config;
using Microsoft.Extensions.Options;

namespace GeminiServer.Gemini;

public class GeminiProtocolHandler(IOptions<NetworkConfig> config) : IProtocolHandler<GeminiRequest, GeminiResponse>
{
    public async Task<GeminiRequest> GetRequestAsync(Stream stream)
    {
        var requestBuffer = new byte[1024 + 2];
        var lastPos = -1;
        var bytesRead = 0;
        while (lastPos < requestBuffer.Length - 1 && (lastPos < 1 || requestBuffer[lastPos - 1] != '\r' || requestBuffer[lastPos] != '\n'))
        {
            bytesRead = await stream.ReadAsync(requestBuffer.AsMemory(lastPos + 1, requestBuffer.Length - lastPos - 1));
            if (bytesRead == 0) break;
            lastPos += bytesRead;
        }

        if (lastPos < 1 || requestBuffer[lastPos - 1] != '\r' || requestBuffer[lastPos] != '\n')
        {
            if (bytesRead == 0) throw new GeminiException(GeminiStatusCodes.Null);
            throw new GeminiException(GeminiStatusCodes.BadRequest, "Invalid request length");
        }
        
        string requestString;
        try
        {
            var decoder = new UTF8Encoding(false, true);
            requestString = await Task.Run(() => decoder.GetString(requestBuffer.AsSpan(0, lastPos - 2 + 1)));
        }
        catch (Exception ex)
        {
            if (ex is ArgumentException or DecoderFallbackException)
                throw new GeminiException(GeminiStatusCodes.BadRequest, "Invalid request encoding");
            throw;
        }

        Uri uri;
        try
        {
            uri = new Uri(requestString, UriKind.Absolute);
        }
        catch (UriFormatException)
        {
            throw new GeminiException(GeminiStatusCodes.BadRequest, "Invalid request URI");
        }
        
        if (string.IsNullOrEmpty(uri.Scheme)) throw new GeminiException(GeminiStatusCodes.BadRequest, "Missing URI scheme");
        if (!string.Equals(uri.Scheme, "gemini", StringComparison.OrdinalIgnoreCase)) throw new GeminiException(GeminiStatusCodes.ProxyRequestRefused, $"Unexpected URI scheme: {uri.Scheme}");
        if (!string.IsNullOrEmpty(uri.UserInfo)) throw new GeminiException(GeminiStatusCodes.BadRequest, $"Unexpected URI userInfo: {uri.UserInfo}");
        if (!string.IsNullOrEmpty(uri.Fragment)) throw new GeminiException(GeminiStatusCodes.BadRequest, $"Unexpected URI fragment: {uri.Fragment}");
        if (!string.Equals(uri.Host, config.Value.Hostname, StringComparison.OrdinalIgnoreCase)) throw new GeminiException(GeminiStatusCodes.ProxyRequestRefused, $"Unexpected hostname: {uri.Host}");
        if ((uri.Port == -1 ? 1965 : uri.Port) != config.Value.Port) throw new GeminiException(GeminiStatusCodes.ProxyRequestRefused, $"Unexpected port {uri.Port}");
        
        return new GeminiRequest
        {
            Path = Uri.UnescapeDataString(uri.AbsolutePath),
            QueryString = Uri.UnescapeDataString(uri.Query),
        };
    }

    public async Task SendResponseAsync(Stream stream, GeminiResponse response)
    {
        var encodedHeader = await Task.Run(() => Encoding.UTF8.GetBytes($"{(int)response.StatusCode} {response.Header}\r\n"));
        if (response is { StatusCode: GeminiStatusCodes.Success, Content: null }) throw new ArgumentException("Body of response is null");
        if (response.StatusCode != GeminiStatusCodes.Success || response.Content == null)
        {
            await stream.WriteAsync(encodedHeader);
            return;
        }
        var encodedResponse = new byte[encodedHeader.Length + response.Content!.Value.Length];
        encodedHeader.CopyTo(encodedResponse);
        response.Content.Value.Span.CopyTo(encodedResponse.AsSpan(encodedHeader.Length));
        await stream.WriteAsync(encodedResponse);
    }
}