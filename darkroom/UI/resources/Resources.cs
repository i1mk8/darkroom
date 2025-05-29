using System.Reflection;

namespace darkroom.UI.resources;

/// <summary>
/// Распаковывает ресурсы
/// </summary>
public static class Resources
{
    // Звуки
    private const string WalkSoundResource = "darkroom.UI.resources.sounds.WalkSound.wav";
    public const string WalkSoundPath = "WalkSound.wav";

    private const string ShootSoundResource = "darkroom.UI.resources.sounds.ShootSound.wav";
    public const string ShootSoundPath = "ShootSound.wav";

    private const string HitSoundResource = "darkroom.UI.resources.sounds.HitSound.wav";
    public const string HitSoundPath = "HitSound.wav";

    private const string TakeShotSoundResource = "darkroom.UI.resources.sounds.TakeShotSound.wav";
    public const string TakeShotSoundPath = "TakeShotSound.wav";
    
    private const string SelectSoundResource = "darkroom.UI.resources.sounds.SelectSound.wav";
    public const string SelectSoundPath = "SelectSound.wav";
    
    // Изображения
    private const string IconImageResource = "darkroom.UI.resources.images.icon.ico";
    public const string IconImagePath = "icon.ico";
    
    // Шрифты
    private const string PixelizerFontResource = "darkroom.UI.resources.fonts.PixelizerFont.ttf";
    public const string PixelizerFontPath = "PixelizerFont.ttf";
    
    private const string RobotoFontResource = "darkroom.UI.resources.fonts.RobotoFont.ttf";
    public const string RobotoFontPath = "RobotoFont.ttf";

    private static readonly List<Tuple<string, string>> ResourcesList =
    [
        new(WalkSoundResource, WalkSoundPath),
        new(ShootSoundResource, ShootSoundPath),
        new(HitSoundResource, HitSoundPath),
        new(TakeShotSoundResource, TakeShotSoundPath),
        new(SelectSoundResource, SelectSoundPath),
        
        new(IconImageResource, IconImagePath),
        
        new(PixelizerFontResource, PixelizerFontPath),
        new(RobotoFontResource, RobotoFontPath)
    ];

    /// <summary>
    /// Распаковывает встроенный ресурс на диск
    /// </summary>
    /// <param name="name">Интендификатор встроенного ресурса</param>
    /// <param name="destination">Имя распакованного ресурса на диске</param>
    private static void Extract(string name, string destination)
    {
        try
        {
            using var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
            using var file = new FileStream(destination, FileMode.Create, FileAccess.Write);
            resource.CopyTo(file);
        }
        catch (IOException) { }
    }
    
    /// <summary>
    /// Распаковывает все ресурсы (из ResourcesList)
    /// </summary>
    public static void ExtractAll()
    {
        foreach (var resource in ResourcesList)
            Extract(resource.Item1, resource.Item2);
    }
}