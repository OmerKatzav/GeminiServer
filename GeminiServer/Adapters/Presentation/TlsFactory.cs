using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using GeminiServer.Abstractions;
using GeminiServer.Config;
using Microsoft.Extensions.Options;

namespace GeminiServer.Adapters.Presentation;

public class TlsFactory(IOptions<NetworkConfig> config) : IPresentationFactory
{
    private readonly X509Certificate2 _certificate = X509CertificateLoader.LoadPkcs12(X509Certificate2.CreateFromPemFile(config.Value.CertificateFile, config.Value.CertificateKeyFile).Export(X509ContentType.Pkcs12), password: null);

    public IPresentation Create(Stream stream, IDictionary<string, object?> metadata)
    {
        var tlsStream = new SslStream(stream, false, (_, _, _, _) => true);
        tlsStream.AuthenticateAsServer(_certificate);
        metadata["ClientCertificateHash"] = tlsStream.RemoteCertificate?.GetCertHash();
        metadata["ClientCertificateSubject"] = tlsStream.RemoteCertificate?.Subject;
        metadata["ClientCertificateIssuer"] = tlsStream.RemoteCertificate?.Issuer;
        return new Tls(tlsStream);
    }
}