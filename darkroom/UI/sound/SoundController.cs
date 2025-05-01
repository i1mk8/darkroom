using darkroom.model;
using darkroom.model.player;
using darkroom.utils;

namespace darkroom.UI.sound;

/// <summary>
/// Контроллер для управления звуковыми эффектами
/// </summary>
public class SoundController(Player mainPlayer)
{
    private const string WalkSoundResource = "darkroom.UI.resources.WalkSound.wav";
    private const string WalkSoundPath = "WalkSound.wav";
    
    private const string ShootSoundResource = "darkroom.UI.resources.ShootSound.wav";
    private const string ShootSoundPath = "ShootSound.wav";
    
    private const float MaxWalkSoundDistance = 10;
    private const float ShootSoundCoefficient = 0.5f;
    
    private readonly Sound _walkSound = new(WalkSoundResource, WalkSoundPath);
    private readonly Sound _shootSound = new(ShootSoundResource, ShootSoundPath);
    
    /// <summary>
    /// Воспроизводит звук шагов игрока
    /// </summary>
    /// <param name="originPlayer">Игрок, вызвавший звук шагов</param>
    public void PlayWalkSound(Player originPlayer)
    {
        if (originPlayer == mainPlayer)
            return;
        
        var distance = originPlayer.Box.DistanceTo(mainPlayer.Box);
        if (distance > MaxWalkSoundDistance)
            return;
        
        _walkSound.PlaySoundOnce(1);
    }
    
    /// <summary>
    /// Воспроизводит звук выстрела игрока
    /// </summary>
    /// <param name="shooter">Игрок, совершивший выстрел</param>
    public void PlayShootSound(Player shooter)
    {
        float volume;

        if (shooter == mainPlayer)
            volume = 1f;
        else
        {
            var distance = shooter.Box.DistanceTo(mainPlayer.Box);
            volume = 1f / (1f + distance * ShootSoundCoefficient);
        }
        
        _shootSound.PlaySound(volume);
    }
}