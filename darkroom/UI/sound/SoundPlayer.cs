using darkroom.utils;
using NAudio.Wave;

namespace darkroom.UI.sound;

public class SoundPlayer
{
    private readonly AudioFileReader _shootSound;
    private readonly WaveOutEvent _shootOut;
    
    private readonly AudioFileReader _walkSound;
    private readonly WaveOutEvent _walkOut;

    public SoundPlayer()
    {
        const string shootSoundResource = "darkroom.UI.resources.ShootSound.wav";
        const string shootSoundPath = "ShootSound.wav";
        
        const string walkSoundResource = "darkroom.UI.resources.WalkSound.wav";
        const string walkSoundPath = "WalkSound.wav";
        
        Utils.ExtractResource(shootSoundResource, shootSoundPath);
        Utils.ExtractResource(walkSoundResource, walkSoundPath);
            
        _shootSound = new AudioFileReader(shootSoundPath);
        _shootOut = new WaveOutEvent();
        _shootOut.Init(_shootSound);
        
        _walkSound = new AudioFileReader(walkSoundPath);
        _walkOut = new WaveOutEvent();
        _walkOut.Init(_walkSound);
    }

    private void PlaySound(AudioFileReader sound, WaveOutEvent waveOut, float volume)
    {
        sound.Position = 0;
        sound.Volume = volume;
        waveOut.Play();
    }

    public void PlayShootSound(float volume) => PlaySound(_shootSound, _shootOut, volume);
    
    public void PlayWalkSound(float volume)
    {
        if (_walkOut.PlaybackState == PlaybackState.Stopped)
            PlaySound(_walkSound, _walkOut, volume);
    }
}