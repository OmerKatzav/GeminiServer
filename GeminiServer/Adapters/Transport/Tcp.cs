using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using GeminiServer.Abstractions;
using GeminiServer.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GeminiServer.Adapters.Transport;

public class Tcp(IOptions<NetworkConfig> config, IPresentationFactory presentationFactory, IClientHandler clientHandler, ILogger<Tcp> logger) : ITransport
{
    private readonly TcpListener _listener = new(IPAddress.IPv6Any, config.Value.Port);
    private readonly CancellationTokenSource _cts = new();

    public async Task StartAsync(CancellationToken ct)
    {
        _listener.Server.DualMode = true;
        _listener.Start();
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Listening on {LocalEndPoint}", _listener.LocalEndpoint);
        }
        var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, ct);
        while (!combinedCts.Token.IsCancellationRequested)
        {
            try
            {
                ct.ThrowIfCancellationRequested();
                var client = await _listener.AcceptTcpClientAsync(combinedCts.Token);
                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation("Accepted connection from {RemoteEndPoint}", client.Client.RemoteEndPoint);
                }
                _ = HandleClientAsync(client, combinedCts.Token);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured while handling client");
            }
        }
    }

    private async Task HandleClientAsync(TcpClient client, CancellationToken ct)
    {
        try
        {
            var metadata = new ConcurrentDictionary<string, object?>
            {
                ["ClientIpEndpoint"] = client.Client.RemoteEndPoint
            };
            var presentation = presentationFactory.Create(client.GetStream(), metadata);
            await clientHandler.HandleAsync(presentation.Stream, metadata, ct);
            await presentation.StopAsync();
            await presentation.Stream.DisposeAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occured while handling client");
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Stopping client");
        }
        await _cts.CancelAsync();
        _cts.Dispose();
        _listener.Stop();
        _listener.Dispose();
    }
}