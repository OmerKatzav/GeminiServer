namespace GeminiServer.Config;

public class NetworkConfig
{
    public string Hostname { get; set; } = "localhost";
    public int Port { get; set; } = 1965;
    public string CertificateFile { get; set; } = "certs/cert.pem";
    public string CertificateKeyFile { get; set; } = "certs/key.pem";
}