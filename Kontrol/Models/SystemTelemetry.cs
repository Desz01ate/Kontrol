namespace Kontrol.Models;

public enum MediaPlayerState
{
    Paused,
    Playing,
    Stopped,
    Unknown,
}

public sealed record SystemTelemetry(uint Volume, MediaPlayerState State)
{
    public static readonly SystemTelemetry Unknown = new(0u, MediaPlayerState.Unknown);
}