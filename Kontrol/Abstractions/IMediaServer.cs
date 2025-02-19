using Kontrol.Models;

namespace Kontrol.Abstractions;

public interface IMediaServer
{
    Task ChangeVolume(uint volume);

    Task TogglePlayPause();

    Task MediaNext();

    Task MediaPrevious();

    Task ToggleMute();

    Task<SystemTelemetry> GetSystemTelemetry();
}