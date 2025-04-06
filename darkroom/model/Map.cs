namespace darkroom.model;

/// <summary>
///  Игровая карта
/// </summary>
/// <param name="width">Длина карты</param>
/// <param name="height">Ширина карты</param>
/// <param name="walls">Стены, расположенные на карте</param>
public class Map(int width, int height, List<RectangleF> walls)
{
    public readonly int Width = width;
    public readonly int Height = height;
    public readonly List<RectangleF> Walls = walls;
    
    /// <summary>
    /// Создает игровую карту
    /// </summary>
    /// <param name="width">Длина карты</param>
    /// <param name="height">Ширина карты</param>
    /// <param name="wallOffset">Расстояние между стенами</param>
    /// <param name="minWallSize">Минимальный размер стены (может не соблюдаться при выходе за границы карты)</param>
    /// <param name="maxWallSize">Максимальный размер стены</param>
    /// <returns>Игровая карта</returns>
    public static Map Generate(int width,
        int height,
        int wallOffset,
        int minWallSize,
        int maxWallSize)
    {
        var walls = new List<RectangleF>();
        var random = new Random();

        // Рассчитываем количество стен по горизонтали и вертикали
        var cols = (int)Math.Ceiling((double)(width + wallOffset) / (maxWallSize + wallOffset));
        var rows = (int)Math.Ceiling((double)(height + wallOffset) / (maxWallSize + wallOffset));
        
        // Рассчитываем начальные отступы для центровки
        var totalWallWidth = cols * maxWallSize + (cols - 1) * wallOffset;
        var totalWallHeight = rows * maxWallSize + (rows - 1) * wallOffset;
        var startOffsetX = Math.Max(0, (width - totalWallWidth) / 2);
        var startOffsetY = Math.Max(0, (height - totalWallHeight) / 2);

        for (var col = 0; col < cols; col++)
        {
            for (var row = 0; row < rows; row++)
            {
                // Вычисляем стартовые координаты с учетом отступов и центровки
                var startX = startOffsetX + col * (maxWallSize + wallOffset);
                var startY = startOffsetY + row * (maxWallSize + wallOffset);
                
                // Проверяем, чтобы стена не выходила за границы карты
                if (startX >= width || startY >= height)
                    continue;
                
                var wallSize = random.Next(minWallSize, maxWallSize + 1);

                int endX;
                int endY;
                var wallStartX = startX;
                var wallStartY = startY;
                
                if (random.Next(0, 2) == 0)
                {
                    // Вертикальная стена
                    endX = startX + maxWallSize / 2;
                    endY = startY + wallSize;
                    wallStartX = endX - 1;
                }
                else
                {
                    // Горизонтальная стена
                    endX = startX + wallSize;
                    endY = startY + maxWallSize / 2;
                    wallStartY = endY - 1;
                }
                
                // Корректируем конечные координаты, если выходим за границы
                endX = Math.Min(endX, width);
                endY = Math.Min(endY, height);
                
                var wall = RectangleF.FromLTRB(wallStartX, wallStartY, endX, endY);
                walls.Add(wall);
                Console.WriteLine($"Wall: {wall}");
            }
        }

        return new Map(width, height, walls);
    }
    
    /// <summary>
    /// Ищет пересечение объекта со стенами или выход за пределы игровой карты
    /// </summary>
    /// <param name="box">Бокс объекта</param>
    /// <returns>Стена или граница, с которой пересекается объект (если пересечение существует)</returns>
    public RectangleF? FindIntersect(RectangleF box)
    {
        // Границы карты представляются как стены
        if (box.Left < 0)
            return new RectangleF(0, 0, 0, Height);
        if (box.Right > Width)
            return new RectangleF(Width, 0, 0, Height);
        if (box.Top < 0)
            return new RectangleF(0, 0, Width, 0);
        if (box.Bottom > Height)
            return new RectangleF(0, Height, Width, 0);

        foreach (var wall in Walls)
            if (wall.IntersectsWith(box))
                return wall;

        return null;
    }
    
    /// <summary>
    /// Ищет пересечение точки со стенами или выход за пределы игровой карты
    /// </summary>
    /// <param name="point">Точка</param>
    /// <returns>Стена или граница, с которой пересекается точка (если пересечение существует)</returns>
    public RectangleF? FindIntersect(PointF point)
    {
        return FindIntersect(new RectangleF(point.X, point.Y, 0, 0));
    }
}