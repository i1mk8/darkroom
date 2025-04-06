namespace darkroom.model;

/// <summary>
/// Игрок
/// </summary>
/// <param name="map">Игровая карта</param>
/// <param name="width">Длина игрока</param>
/// <param name="height">Ширина игрока</param>
/// <param name="speed">Скорость игрока</param>
public class Player(Map map, int width, int height, float speed)
{
    public RectangleF Box { get; private set; } = new(-1, -1, width, height);
    public Fov Fov { get; private set; }


    public void Initialize()
    {
        const float viewDistance = 10f;
        const float viewAngle = 90f;
        const float baseAngleSpeed = 5f;
        
        SpawnPlayer();
        Fov = new Fov(map, this, viewDistance, viewAngle, baseAngleSpeed);
    }

    /// <summary>
    /// Перемещает игрока в заданные координаты, если это возможно
    /// </summary>
    /// <param name="x">Координата по X</param>
    /// <param name="y">Координата по Y</param>
    /// <returns>Объект, помешавший перемещению (если он была)</returns>
    public RectangleF? MoveTo(float x, float y)
    {
        var box = new RectangleF(x, y, width, height);
        var intersect = map.FindIntersect(box);

        if (intersect != null)
            return intersect;

        Box = box;
        return null;
    }

    /// <summary>
    /// Перемещение игрока вперед
    /// </summary>
    public void MoveForward()
    {
        var intersect = MoveTo(Box.X, Box.Y + speed);
        if (intersect != null)
            MoveTo(Box.X, Box.Y + (intersect.Value.Top - Box.Bottom));
    }
    
    /// <summary>
    /// Перемещение игрока назад
    /// </summary>
    public void MoveBack()
    {
        var intersect = MoveTo(Box.X, Box.Y - speed);
        if (intersect != null) 
            MoveTo(Box.X, Box.Y - (Box.Top - intersect.Value.Bottom));
    }

    /// <summary>
    /// Перемещение игрока вправо
    /// </summary>
    public void MoveRight()
    {
        var intersect = MoveTo(Box.X + speed, Box.Y);
        if (intersect != null)
            MoveTo(Box.X + (intersect.Value.Left - Box.Right), Box.Y);
    }

    /// <summary>
    /// Перемещение игрока влево
    /// </summary>
    public void MoveLeft()
    {
        var intersect = MoveTo(Box.X - speed, Box.Y);
        if (intersect != null)
            MoveTo(Box.X - (Box.Left - intersect.Value.Right), Box.Y);
    }
    
    /// <summary>
    /// Спавнит игрока в рандомной точке игровой карты
    /// </summary>
    public void SpawnPlayer()
    {
        var random = new Random();
        
        var minX = Box.Height;
        var maxX = map.Width - Box.Width;
        
        var minY = Box.Height;
        var maxY = map.Height - Box.Height;
        
        while (true)
        {
            var x = Math.Clamp(random.Next(0, map.Width + 1), minX, maxX);
            var y = Math.Clamp(random.Next(0, map.Height + 1), minY, maxY);
            
            if (MoveTo(x, y) == null)
                break;
        }
        
        Console.WriteLine($"Player: {Box}");
    }
}