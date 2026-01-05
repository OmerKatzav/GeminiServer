namespace GeminiServer.Gemini;

public class GeminiException : Exception
{
    public GeminiStatusCodes StatusCode { get; } = GeminiStatusCodes.TempFailure;
    public string ErrorMessage { get; } = string.Empty;
    
    public GeminiException() {}

    public GeminiException(string message) : base(message) { ErrorMessage = message; }
    
    public GeminiException(string message, Exception inner) : base(message, inner) { ErrorMessage = message; }

    public GeminiException(GeminiStatusCodes statusCode) : base(statusCode.ToString()) { StatusCode = statusCode; }
    
    public GeminiException(GeminiStatusCodes statusCode, Exception inner) : base(statusCode.ToString(), inner) { StatusCode = statusCode; }
    
    public GeminiException(GeminiStatusCodes statusCode, string message) : base($"{statusCode}: {message}") { StatusCode = statusCode; ErrorMessage = message; }
    
    public GeminiException(GeminiStatusCodes statusCode, string message, Exception inner) : base($"{statusCode}: {message}", inner) { StatusCode = statusCode; ErrorMessage = message; }
}