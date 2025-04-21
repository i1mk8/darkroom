using darkroom.utils;

namespace darkroom.model;

/// <summary>
/// Пуля
/// </summary>
public class Bullet
{
    public readonly Player OriginPlayer;
    
    public readonly PointF Direction;
    public RectangleF Box;
    public readonly float Speed;
    
    /// <summary>
    /// Конструктор пули
    /// </summary>
    /// <param name="originPlayer">Стреляющий игрок</param>
    /// <param name="width">Длина пули</param>
    /// <param name="height">Ширина пули</param>
    /// <param name="speed">Скорость пули</param>
    public Bullet(Player originPlayer, float width, float height, float speed)
    {
        OriginPlayer = originPlayer;
        
        Direction = new PointF(MathF.Cos(Utils.ToRadians(originPlayer.Fov.BaseAngle)),
            MathF.Sin(Utils.ToRadians(originPlayer.Fov.BaseAngle)));
        Box = new RectangleF(Direction.X + originPlayer.Box.Center().X,
            Direction.Y + originPlayer.Box.Center().Y, width, height);
        Speed = speed;
    }
    
    /// <summary>
    /// Перемещает пулю в заданные координаты
    /// </summary>
    /// <param name="x">Координата по X</param>
    /// <param name="y">Координата по Y</param>
    public void MoveTo(float x, float y) => Box = Box with { X = x, Y = y };
}