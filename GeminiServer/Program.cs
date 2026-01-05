using GeminiServer;
using GeminiServer.Core;
using GeminiServer.Gemini;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = GeminiApp.CreateApplicationBuilder();
builder.Services.AddSingleton<IMimeService, MimeService>();
var app = builder.Build();
app.UseMiddleware<GeminiStaticFileMiddleware>();
await app.RunAsync();
