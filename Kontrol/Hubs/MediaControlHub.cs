using Kontrol.Abstractions;
using Kontrol.Models;
using Microsoft.AspNetCore.SignalR;

namespace Kontrol.Hubs;

public interface IClient
{
    Task ResponseSystemTelemetry(SystemTelemetry telemetry);
}

public class MediaControlHub : Hub<IClient>, IMediaServer // For ws interfacing only
{
    private readonly IMediaServer mediaServer;
    private readonly ILogger<MediaControlHub> logger;

    public MediaControlHub(
        IMediaServer mediaServer,
        ILogger<MediaControlHub> logger)
    {
        this.mediaServer = mediaServer;
        this.logger = logger;
    }

    public override Task OnConnectedAsync()
    {
        // TODO: implement security mechanism.

        return base.OnConnectedAsync();
    }

    public async Task ChangeVolume(uint volume)
    {
        await this.mediaServer.ChangeVolume(volume);
        await this.Clients.All.ResponseSystemTelemetry(await this.mediaServer.GetSystemTelemetry());
    }

    public async Task TogglePlayPause()
    {
        await this.mediaServer.TogglePlayPause();
        await this.Clients.All.ResponseSystemTelemetry(await this.mediaServer.GetSystemTelemetry());
    }

    public async Task MediaNext()
    {
        await this.mediaServer.MediaNext();
        await this.Clients.All.ResponseSystemTelemetry(await this.mediaServer.GetSystemTelemetry());
    }

    public async Task MediaPrevious()
    {
        await this.mediaServer.MediaPrevious();
        await this.Clients.All.ResponseSystemTelemetry(await this.mediaServer.GetSystemTelemetry());
    }

    public async Task ToggleMute()
    {
        await this.mediaServer.ToggleMute();
        await this.Clients.All.ResponseSystemTelemetry(await this.mediaServer.GetSystemTelemetry());
    }

    public Task<SystemTelemetry> GetSystemTelemetry()
    {
        return this.mediaServer.GetSystemTelemetry();
    }
}