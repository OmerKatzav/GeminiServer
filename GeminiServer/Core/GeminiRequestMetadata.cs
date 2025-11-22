using System.Net;

namespace GeminiServer.Core;

public class GeminiRequestMetadata
{
    public required IPEndPoint IpEndPoint { get; set; }
    public byte[]? ClientCertificateHash { get; set; }
    public string? ClientCertificateSubject { get; set; }
    public string? ClientCertificateIssuer { get; set; }
}