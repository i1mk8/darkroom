namespace darkroom.utils;

/// <summary>
/// Расширение для RectangleF
/// </summary>
public static class RectangleExtension
{
    
    /// <summary>
    /// Считает центр прямоугольника
    /// </summary>
    /// <param name="rectangle">Прямоугольник</param>
    /// <returns>Точка, находящаеся в центре прямоугольника</returns>
    public static PointF Center(this RectangleF rectangle) => new(rectangle.X + rectangle.Width / 2,
        rectangle.Y + rectangle.Height / 2);

    public static Point DecimalCords(this RectangleF rectangle) => new((int)rectangle.X, (int)rectangle.Y);
    
    /// <summary>
    /// Считает дистанцию от 1-го прямоугольника до 2-го
    /// </summary>
    /// <param name="from">Прямоугольник 1</param>
    /// <param name="to">Прямоугольник 2</param>
    /// <returns>Дистанция от 1-го прямоугольника до 2-го</returns>
    public static float DistanceTo(this RectangleF from, RectangleF to)
    {
        var centerFrom = from.Center();
        var centerTo = to.Center();
        
        var dx = centerFrom.X - centerTo.X;
        var dy = centerFrom.Y - centerTo.Y;
        
        return (float)Math.Sqrt(dx * dx + dy * dy);
    }
}