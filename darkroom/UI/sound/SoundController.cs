using darkroom.model;
using darkroom.utils;

namespace darkroom.UI.sound;

public class SoundController(Player mainPlayer)
{
    private readonly SoundPlayer _soundPlayer = new();

    public void PlayWalkSound(Player originPlayer)
    {
        if (originPlayer == mainPlayer)
            return;
        
        const float maxDistance = 10f;
        var distance = originPlayer.Box.DistanceTo(mainPlayer.Box);
        if (distance > maxDistance)
            return;
        
        _soundPlayer.PlayWalkSound(1);
    }

    public void PlayShootSound(Player shooter)
    {
        float volume;

        if (shooter == mainPlayer)
            volume = 1f;
        else
        {
            var distance = shooter.Box.DistanceTo(mainPlayer.Box);
            volume = 1f / (1f + distance * 0.5f);
        }
        
        _soundPlayer.PlayShootSound(volume);
    }
}