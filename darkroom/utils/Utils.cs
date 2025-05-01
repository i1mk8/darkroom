using System.Reflection;
using Microsoft.Extensions.Logging;

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
    /// Переводит угол из радиан в градусы
    /// </summary>
    /// <param name="angle">Угол в радианах</param>
    /// <returns>Угол в градусах</returns>
    public static float ToDegrees(float angle) => angle * 180 / (float)Math.PI;

    /// <summary>
    /// Распаковывает встроенный ресурс на диск
    /// </summary>
    /// <param name="resourceName">Интендификатор встроенного ресурса</param>
    /// <param name="destination">Имя распакованного ресурса на диске</param>
    public static void ExtractResource(string resourceName, string destination)
    {
        using var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        try
        {
            using var file = new FileStream(destination, FileMode.Create, FileAccess.Write);
            resource.CopyTo(file);
        }
        catch (IOException) {}
    }
    
    /// <summary>
    /// Неточное проверка равенства двух чисел
    /// </summary>
    /// <param name="num1">Число 1</param>
    /// <param name="num2">Число 2</param>
    /// <param name="delta">Максимальная погрешность</param>
    public static bool InaccurateEquals(float num1, float num2, float delta) => Math.Abs(num1 - num2) <= delta;

    /// <summary>
    /// Фабрика для создания объектов логирования
    /// </summary>
    public static readonly ILoggerFactory LoggerFactory =
        Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
            builder.AddConsole().SetMinimumLevel(LogLevel.Debug));
}