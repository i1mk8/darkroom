namespace darkroom.model;

/// <summary>
/// Игрок
/// </summary>
/// <param name="width">Длина игрока</param>
/// <param name="height">Ширина игрока</param>
public class Player(int width, int height)
{
    public RectangleF Box { get; private set; } = new(-1, -1, width, height);

    /// <summary>
    /// Перемещает игрока в заданные координаты
    /// </summary>
    /// <param name="x">Координата по X</param>
    /// <param name="y">Координата по Y</param>
    public void MoveTo(float x, float y)
    {
        Box = new RectangleF(x, y, width, height);
    }
}