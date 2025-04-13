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
}