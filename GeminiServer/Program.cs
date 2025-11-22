using GeminiServer;
using GeminiServer.Adapters;
using GeminiServer.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = Host.CreateApplicationBuilder();
builder
    .Configure()
    .UseTls();

builder.Services.AddSingleton<IRequestHandler, GeminiRequestParser>();
builder.Services.AddSingleton<IGeminiRequestHandler, StaticFileRequestHandler>();
builder.Services.AddSingleton<IMimeService, MimeService>();

var app = builder.Build();
await app.RunAsync();
