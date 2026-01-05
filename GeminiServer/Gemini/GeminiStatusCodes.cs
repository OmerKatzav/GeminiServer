namespace GeminiServer.Gemini;

public enum GeminiStatusCodes
{
    Null = 0,
    
    InputExpected = 10,
    SensitiveInputExpected = 11,
    
    Success = 20,
    
    TempRedirect = 30,
    PermanentRedirect = 31,
    
    TempFailure = 40,
    ServerUnavailable = 41,
    CgiError = 42,
    ProxyError = 43,
    SlowDown = 44,
    
    PermanentFailure = 50,
    NotFound = 51,
    Gone = 52,
    ProxyRequestRefused = 53,
    BadRequest = 59,
    
    CertificateRequired = 60,
    CertificateUnauthorized = 61,
    CertificateInvalid = 62
}