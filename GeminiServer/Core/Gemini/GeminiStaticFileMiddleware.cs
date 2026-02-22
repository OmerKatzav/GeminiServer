using GeminiServer.Abstractions;
using GeminiServer.Config;
using Microsoft.Extensions.Options;

namespace GeminiServer.Core.Gemini;

public class GeminiStaticFileMiddleware(IOptions<StaticFileConfig> config, IMimeService mimeService) : IMiddleware<State<GeminiRequest, GeminiResponse>>
{
    public async Task InvokeAsync(State<GeminiRequest, GeminiResponse> state, InvokeAsync<State<GeminiRequest, GeminiResponse>> next)
    {
        if (state.Request.Path.IndexOfAny(Path.GetInvalidPathChars()) != -1) throw new GeminiException(GeminiStatusCodes.BadRequest, "Invalid path");
        
        var fullBasePath = Path.GetFullPath(config.Value.Directory); 
        if (!fullBasePath.EndsWith(Path.DirectorySeparatorChar)) fullBasePath += Path.DirectorySeparatorChar;
        var path = Path.GetFullPath(Path.Combine(fullBasePath, state.Request.Path.TrimStart('/', '\\'))).Replace('/', Path.DirectorySeparatorChar);
        var comparison = OperatingSystem.IsWindows()
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;
        if (!path.StartsWith(fullBasePath, comparison)) throw new GeminiException(GeminiStatusCodes.BadRequest, "Invalid path");

        foreach (var availablePath in new[] { path, Path.Combine(path, "index.gmi"), Path.Combine(path, "index.gemini") })
        {
            if (!File.Exists(availablePath)) continue;
            state.Response = await ServeFile(availablePath);
            return;
        }
        await next(state);
    }

    private async Task<GeminiResponse> ServeFile(string fileName)
    {
        var mimeType = mimeService.GetMimeType(fileName);
        var content = await File.ReadAllBytesAsync(fileName);
        return new GeminiResponse{Header = mimeType, Content = content};
    }
}