using NAudio.Wave;

namespace darkroom.UI.sound;

/// <summary>
/// Воспроизводит звуковые файлы
/// </summary>
public class Sound
{
    private readonly AudioFileReader _audioFileReader;
    private readonly WaveOutEvent _waveOut;
    
    /// <param name="path">Путь до звукового файла</param>
    public Sound(string path)
    {
        _audioFileReader = new AudioFileReader(path);
        _waveOut = new WaveOutEvent();
        _waveOut.Init(_audioFileReader);
    }

    /// <summary>
    /// Воспроизводит звук с указанной громкостью
    /// </summary>
    /// <param name="volume">Уровень громкости</param>
    public void PlaySound(float volume)
    {
        _audioFileReader.Position = 0;
        _audioFileReader.Volume = volume;
        _waveOut.Play();
    }
    
    /// <summary>
    /// Воспроизводит звук только если он не воспроизводится в данный момент
    /// </summary>
    /// <param name="volume">Уровень громкости</param>
    public void PlaySoundOnce(float volume)
    {
        if (_waveOut.PlaybackState == PlaybackState.Stopped)
            PlaySound(volume);
    }
}