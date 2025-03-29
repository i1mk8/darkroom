namespace darkroom.model;

/// <summary>
/// Класс, предстовляющий игрока
/// </summary>
/// <param name="width">Длина игрока</param>
/// <param name="height">Ширина игрока</param>
public class Player(int width, int height)
{
    public Rectangle Box { get; private set; }
    
    /// <summary>
    /// Перемещает игрока в заданные координаты
    /// </summary>
    /// <param name="x">Координата по X</param>
    /// <param name="y">Координата по Y</param>
    public void MoveTo(int x, int y)
    {
        Box = new Rectangle(x, y, width, height);
    }
}