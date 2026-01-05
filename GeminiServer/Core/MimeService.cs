using Microsoft.AspNetCore.StaticFiles;
using MimeDetective;

namespace GeminiServer.Core;

public class MimeService : IMimeService
{
    private readonly FileExtensionContentTypeProvider _mimeExtensionInspector = new();
    private readonly IContentInspector _mimeContentInspector = new ContentInspectorBuilder
    {
        Definitions = new MimeDetective.Definitions.ExhaustiveBuilder
        {
            UsageType = MimeDetective.Definitions.Licensing.UsageType.PersonalNonCommercial,
        }.Build(),
    }.Build();
    
    public MimeService()
    {
        _mimeExtensionInspector.Mappings[".gmi"] = "text/gemini";
        _mimeExtensionInspector.Mappings[".gemini"] = "text/gemini";
    }
    
    public string GetMimeType(string fileName)
    {
        if (_mimeExtensionInspector.TryGetContentType(fileName, out var mimeType)) return mimeType;
        var result = _mimeContentInspector.Inspect(fileName).ByMimeType().FirstOrDefault();
        return result != null ? result.MimeType : "application/octet-stream";
    }
}