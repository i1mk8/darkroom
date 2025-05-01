using darkroom.utils;
using NAudio.Wave;

namespace darkroom.UI.sound;

/// <summary>
/// Воспроизводит звуковые файлы
/// </summary>
public class Sound
{
    private readonly AudioFileReader _audioFileReader;
    private readonly WaveOutEvent _waveOut;
    
    /// <param name="resource">Имя ресурса звукового файла в сборке</param>
    /// <param name="destination">Путь для сохранения извлечённого звукового файла</param>
    public Sound(string resource, string destination)
    {
        Utils.ExtractResource(resource, destination);
        
        _audioFileReader = new AudioFileReader(destination);
        _waveOut = new WaveOutEvent();
        _waveOut.Init(_audioFileReader);
    }

    /// <summary>
    /// Воспроизводит звук с указанной громкостью
    /// </summary>
    /// <param name="volume">Уровень громкости (от 0.0 до 1.0)</param>
    public void PlaySound(float volume)
    {
        _audioFileReader.Position = 0;
        _audioFileReader.Volume = volume;
        _waveOut.Play();
    }
    
    /// <summary>
    /// Воспроизводит звук только если он не воспроизводится в данный момент
    /// </summary>
    /// <param name="volume">Уровень громкости (от 0.0 до 1.0)</param>
    public void PlaySoundOnce(float volume)
    {
        if (_waveOut.PlaybackState == PlaybackState.Stopped)
            PlaySound(volume);
    }
}