namespace darkroom.utils;

public static class PointExtension
{
    /// <summary>
    /// Считает дистанцию от 1-ой точки до 2-ой
    /// </summary>
    /// <param name="from">Точка 1</param>
    /// <param name="to">Точка 2</param>
    /// <returns>Дистанция от 1-ой точки до 2-ой</returns>
    public static float DistanceTo(this PointF from, PointF to) =>
        MathF.Sqrt(MathF.Pow(from.X - to.X, 2) + MathF.Pow(from.Y - to.Y, 2));
    
    /// <summary>
    /// Нормализация вектора направления
    /// </summary>
    /// <param name="direction">Направление</param>
    /// <param name="length">Длина вектора</param>
    /// <returns>Нормализованое направление</returns>
    public static PointF NormalizeDirection(this PointF direction, float length) =>
        length > 0 ? new PointF(direction.X / length, direction.Y / length) : direction;
}