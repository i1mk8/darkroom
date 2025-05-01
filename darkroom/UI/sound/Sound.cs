using darkroom.utils;
using NAudio.Wave;

namespace darkroom.UI.sound;

public class Sound
{
    private readonly AudioFileReader _audioFileReader;
    private readonly WaveOutEvent _waveOut;
    
    public Sound(string resource, string destination)
    {
        Utils.ExtractResource(resource, destination);
        
        _audioFileReader = new AudioFileReader(destination);
        _waveOut = new WaveOutEvent();
        _waveOut.Init(_audioFileReader);
    }

    public void PlaySound(float volume)
    {
        _audioFileReader.Position = 0;
        _audioFileReader.Volume = volume;
        _waveOut.Play();
    }

    public void PlaySoundOnce(float volume)
    {
        if (_waveOut.PlaybackState == PlaybackState.Stopped)
            PlaySound(volume);
    }
}