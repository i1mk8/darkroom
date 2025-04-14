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
    
    /// <summary>
    /// Считает дистанцию от 1-го прямоугольника до 2-го
    /// </summary>
    /// <param name="from">Прямоугольник 1</param>
    /// <param name="to">Прямоугольник 2</param>
    /// <returns>Дистанция от 1-го прямоугольника до 2-го</returns>
    public static float DistanceTo(this RectangleF from, RectangleF to)
    {
        var dx = from.CenterX() - to.CenterX();
        var dy = from.CenterY() - to.CenterY();
        return (float)Math.Sqrt(dx * dx + dy * dy);
    }
}