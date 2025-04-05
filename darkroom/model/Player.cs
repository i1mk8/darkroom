namespace darkroom.model;

/// <summary>
/// Игрок
/// </summary>
/// <param name="width">Длина игрока</param>
/// <param name="height">Ширина игрока</param>
/// <param name="map">Игровая карта</param>
public class Player(Map map, int width, int height, float speed)
{
    public RectangleF Box { get; private set; } = new(-1, -1, width, height);

    /// <summary>
    /// Перемещает игрока в заданные координаты
    /// </summary>
    /// <param name="x">Координата по X</param>
    /// <param name="y">Координата по Y</param>
    /// <returns>true - перемещение успешно; false - перемещение невозможно</returns>
    public bool MoveTo(float x, float y)
    {
        var box = new RectangleF(x, y, width, height);
        if (!map.IsWithin(box))
            return false;
        Box = box;
        return true;
    }

    /// <summary>
    /// Перемещение игрока вперед
    /// </summary>
    public void MoveForward() => MoveTo(Box.X, Box.Y + speed);
    /// <summary>
    /// Перемещение игрока назад
    /// </summary>
    public void MoveBack() => MoveTo(Box.X, Box.Y - speed);
    /// <summary>
    /// Перемещение игрока вправо
    /// </summary>
    public void MoveRight() => MoveTo(Box.X + speed, Box.Y);
    /// <summary>
    /// Перемещение игрока влево
    /// </summary>
    public void MoveLeft() => MoveTo(Box.X - speed, Box.Y);
    

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