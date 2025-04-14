using System.Reflection;

namespace darkroom.utils;

/// <summary>
/// Различные вспомогательные методы
/// </summary>
public static class Utils
{
    /// <summary>
    /// Переводит угол из градусов в радианы
    /// </summary>
    /// <param name="angle">Угол в градусах</param>
    /// <returns>Угол в радианах</returns>
    public static float ToRadians(float angle) => (float)Math.PI * angle / 180;

    /// <summary>
    /// Распаковывает встроенный ресурс на диск
    /// </summary>
    /// <param name="resourceName">Интендификатор встроенного ресурса</param>
    /// <param name="destination">Имя распакованного ресурса на диске</param>
    public static void ExtractResource(string resourceName, string destination)
    {
        using var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        using var file = new FileStream(destination, FileMode.Create, FileAccess.Write);
        resource.CopyTo(file);
    }
}