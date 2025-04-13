namespace darkroom.utils;

/// <summary>
/// Расширение для RectangleF
/// </summary>
public static class RectangleExtension
{
    
    /// <summary>
    /// Считает центр прямоугольника по X
    /// </summary>
    /// <param name="rectangle">Прямоугольник</param>
    /// <returns>Центр прямоугольника по X</returns>
    public static float CenterX(this RectangleF rectangle) => rectangle.X + rectangle.Width / 2;
    
    /// <summary>
    /// Считает центр прямоугольника по Y
    /// </summary>
    /// <param name="rectangle">Прямоугольник</param>
    /// <returns>Центр прямоугольника по Y</returns>
    public static float CenterY(this RectangleF rectangle) => rectangle.Y + rectangle.Width / 2;
}