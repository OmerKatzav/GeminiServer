using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GeminiServer;

public class StaticFileRequestHandler(IOptions<StaticFileConfig> config, IMimeService mimeService, ILogger<StaticFileRequestHandler> logger) : IGeminiRequestHandler
{
    public async Task<GeminiResponse> HandleRequestAsync(GeminiRequest request)
    {
        if (request.Path.IndexOfAny(Path.GetInvalidPathChars()) != -1) throw new GeminiException(GeminiStatusCodes.BadRequest, "Invalid path");
        
        var fullBasePath = Path.GetFullPath(config.Value.Directory); 
        if (!fullBasePath.EndsWith(Path.DirectorySeparatorChar)) fullBasePath += Path.DirectorySeparatorChar;
        var path = Path.GetFullPath(Path.Combine(fullBasePath, request.Path.TrimStart('/', '\\'))).Replace('/', Path.DirectorySeparatorChar);
        var comparison = OperatingSystem.IsWindows()
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;
        if (!path.StartsWith(fullBasePath, comparison)) throw new GeminiException(GeminiStatusCodes.BadRequest, "Invalid path");
            
        if (File.Exists(path)) return await ServeFile(path);

        if (!Directory.Exists(path)) throw new GeminiException(GeminiStatusCodes.NotFound);
        if (File.Exists(Path.Combine(path, "index.gmi"))) return await ServeFile(Path.Combine(path, "index.gmi"));
        if (File.Exists(Path.Combine(path, "index.gemini"))) return await ServeFile(Path.Combine(path, "index.html"));

        throw new GeminiException(GeminiStatusCodes.NotFound);
    }

    private async Task<GeminiResponse> ServeFile(string fileName)
    {
        var mimeType = mimeService.GetMimeType(fileName);
        var content = await File.ReadAllBytesAsync(fileName);
        return new GeminiResponse{MimeType = mimeType, Content = content};
    }
}