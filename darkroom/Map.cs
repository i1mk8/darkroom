namespace darkroom;

public class Map(int width, int height, bool[,] field)
{
    public readonly int Width = width;
    public readonly int Height = height;
    
    /// <summary>
    ///  Игровое поле размером width на height
    ///  true - в клетке находится стена
    ///  false - в клетке пусто
    /// </summary>
    public readonly bool[,] Field = field;
 
    public static Map Generate(int width,
        int height,
        int minWallSize,
        int maxWallSize)
    {
        var field = new bool[width, height];
        var random = new Random();
        
        var minX = width / maxWallSize != 1 ? width / (width / maxWallSize) / 2 : (width - maxWallSize) / 2;
        var minY = height / maxWallSize != 1 ? height / (height / maxWallSize) / 2 : (height - maxWallSize) / 2;
        var maxX = width - maxWallSize;
        var maxY = height - maxWallSize;
        
        for (var startX = minX; startX < maxX; startX += maxWallSize)
            for (var startY = minY; startY < maxY; startY += maxWallSize)
            {
                int endX;
                int endY;
                    
                if (random.Next(0, 2) == 0)
                {
                    // вертикальная стена
                    endX = startX + 1;
                    endY = startY + random.Next(minWallSize, maxWallSize);
                }
                else
                {
                    // горизонтальная стена
                    endX = startX + random.Next(minWallSize, maxWallSize);
                    endY = startY + 1;
                }
                
                Console.WriteLine($"X: {startX}-{endX} Y: {startY}-{endY}");
                    
                for (var x = startX; x < endX; x++)
                    for (var y = startY; y < endY; y++)
                        field[x, y] = true;
            }

        return new Map(width, height, field);
    }
}