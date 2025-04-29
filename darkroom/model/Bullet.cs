using darkroom.utils;

namespace darkroom.model;

/// <summary>
/// Пуля
/// </summary>
public class Bullet
{
    public readonly Player Shooter;
    
    public readonly PointF Direction;
    public RectangleF Box;
    public readonly float Speed;
    
    /// <summary>
    /// Конструктор пули
    /// </summary>
    /// <param name="shooter">Стреляющий игрок</param>
    /// <param name="width">Длина пули</param>
    /// <param name="height">Ширина пули</param>
    /// <param name="speed">Скорость пули</param>
    public Bullet(Player shooter, float width, float height, float speed)
    {
        Shooter = shooter;
        
        Direction = new PointF(MathF.Cos(Utils.ToRadians(shooter.Fov.BaseAngle)),
            MathF.Sin(Utils.ToRadians(shooter.Fov.BaseAngle)));
        Box = new RectangleF(Direction.X + shooter.Box.Center().X - width / 2,
            Direction.Y + shooter.Box.Center().Y - height / 2, width, height);
        Speed = speed;
    }
    
    /// <summary>
    /// Перемещает пулю в заданные координаты
    /// </summary>
    /// <param name="x">Координата по X</param>
    /// <param name="y">Координата по Y</param>
    public void MoveTo(float x, float y) => Box = Box with { X = x, Y = y };
}