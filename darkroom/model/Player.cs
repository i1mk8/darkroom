namespace darkroom.model;

/// <summary>
/// Игрок
/// </summary>
/// <param name="playerWidth">Длина игрока</param>
/// <param name="playerHeight">Ширина игрока</param>
/// <param name="map">Игровая карта</param>
public class Player(int playerWidth, int playerHeight, Map map)
{
    public const float Speed = 0.1f;
    
    public RectangleF Box { get; private set; } = new(-1, -1, playerWidth, playerHeight);

    /// <summary>
    /// Перемещает игрока в заданные координаты
    /// </summary>
    /// <param name="x">Координата по X</param>
    /// <param name="y">Координата по Y</param>
    /// <returns>true - перемещение успешно; false - перемещение невозможно</returns>
    public bool MoveTo(float x, float y)
    {
        var box = new RectangleF(x, y, playerWidth, playerHeight);
        if (!map.IsWithin(box))
            return false;
        Box = box;
        return true;
    }

    /// <summary>
    /// Перемещение игрока вперед
    /// </summary>
    public void MoveForward() => MoveTo(Box.X, Box.Y + Speed);
    /// <summary>
    /// Перемещение игрока назад
    /// </summary>
    public void MoveBack() => MoveTo(Box.X, Box.Y - Speed);
    /// <summary>
    /// Перемещение игрока вправо
    /// </summary>
    public void MoveRight() => MoveTo(Box.X + Speed, Box.Y);
    /// <summary>
    /// Перемещение игрока влево
    /// </summary>
    public void MoveLeft() => MoveTo(Box.X - Speed, Box.Y);
    

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
            
            if (MoveTo(x, y))
                break;
        }
        
        Console.WriteLine($"Player: {Box}");
    }
}