using System.Runtime.InteropServices;
using CoreAudio;
using Kontrol.Abstractions;
using Kontrol.Models;

namespace Kontrol.Implementations;

public class WindowsMediaServer : IMediaServer, IDisposable
{
    [DllImport("user32.dll")]
    private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

    // ref: https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
    private const byte VK_VOLUME_MUTE = 0xAD;
    private const byte VK_MEDIA_NEXT_TRACK = 0xB0;
    private const byte VK_MEDIA_PREV_TRACK = 0xB1;
    private const byte VK_MEDIA_PLAY_PAUSE = 0xB3;

    private readonly MMDeviceEnumerator deviceEnumerator = new();
    private readonly MMDevice audioDevice;

    public WindowsMediaServer()
    {
        audioDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
    }

    public async Task ChangeVolume(uint volume)
    {
        await Task.Run(() =>
        {
            var volumeLevel = Math.Clamp(volume / 100f, 0.0f, 1.0f);
            audioDevice.AudioEndpointVolume!.MasterVolumeLevelScalar = volumeLevel;
        });
    }

    public async Task TogglePlayPause()
    {
        await Task.Run(() => keybd_event(VK_MEDIA_PLAY_PAUSE, 0, 0, 0));
    }

    public async Task MediaNext()
    {
        await Task.Run(() => keybd_event(VK_MEDIA_NEXT_TRACK, 0, 0, 0));
    }

    public async Task MediaPrevious()
    {
        await Task.Run(() => keybd_event(VK_MEDIA_PREV_TRACK, 0, 0, 0));
    }

    public async Task ToggleMute()
    {
        await Task.Run(() => keybd_event(VK_VOLUME_MUTE, 0, 0, 0));
    }

    public Task<SystemTelemetry> GetSystemTelemetry()
    {
        return Task.Run(() =>
        {
            if (audioDevice.AudioEndpointVolume != null)
            {
                var volume = (uint)(audioDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100);

                // TODO: query player state instead of sending unknown.
                return new SystemTelemetry(volume, MediaPlayerState.Unknown);
            }

            return SystemTelemetry.Unknown;
        });
    }

    public void Dispose()
    {
        this.audioDevice.Dispose();
    }
}