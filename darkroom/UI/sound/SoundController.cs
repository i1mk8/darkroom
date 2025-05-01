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
    
    private const string HitSoundResource = "darkroom.UI.resources.HitSound.wav";
    private const string HitSoundPath = "HitSound.wav";
    
    private const string TakeShotSoundResource = "darkroom.UI.resources.TakeShotSound.wav";
    private const string TakeShotSoundPath = "TakeShotSound.wav";
    
    private const float WalkSoundVolume = 4f;
    private const float ShootSoundVolume = 1f;
    private const float HitSoundVolume = 0.8f;
    private const float TakeShotSoundVolume = 1f;
    
    private const float MaxWalkSoundDistance = 10;
    private const float ShootSoundCoefficient = 0.5f;
    
    private readonly Sound _walkSound = new(WalkSoundResource, WalkSoundPath);
    private readonly Sound _shootSound = new(ShootSoundResource, ShootSoundPath);
    private readonly Sound _hitSound = new(HitSoundResource, HitSoundPath);
    private readonly Sound _takeShotSound = new(TakeShotSoundResource, TakeShotSoundPath);
    
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
        
        _walkSound.PlaySoundOnce(WalkSoundVolume);
    }
    
    /// <summary>
    /// Воспроизводит звук выстрела игрока
    /// </summary>
    /// <param name="shooter">Игрок, совершивший выстрел</param>
    public void PlayShootSound(Player shooter)
    {
        var volume = ShootSoundVolume;

        if (shooter != mainPlayer)
        {
            var distance = shooter.Box.DistanceTo(mainPlayer.Box);
            volume = 1f / (1f + distance * ShootSoundCoefficient);
        }
        
        _shootSound.PlaySound(volume);
    }
    
    /// <summary>
    /// Воспроизводит звук попададния в игрока
    /// </summary>
    /// <param name="shooter">Игрок, совершивший выстрел</param>
    public void PlayHitSound(Player shooter)
    {
        if (shooter == mainPlayer)
            _hitSound.PlaySound(HitSoundVolume);
    }

    /// <summary>
    /// Воспроизводит звук получения пули
    /// </summary>
    /// <param name="player">Игрок, в которого попали</param>
    public void PlayTakeShotSound(Player player)
    {
        if (player == mainPlayer)
            _takeShotSound.PlaySound(TakeShotSoundVolume);
    }
}