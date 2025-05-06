using darkroom.utils;
using Microsoft.Extensions.Logging;

namespace darkroom.game;

/// <summary>
///  Игровая карта
/// </summary>
/// <param name="width">Длина карты</param>
/// <param name="height">Ширина карты</param>
/// <param name="walls">Стены, расположенные на карте</param>
public class Map(int width, int height, List<RectangleF> walls)
{
    private static readonly ILogger Logger = Utils.GetLogger<Map>();
    
    private static readonly Random Random = new();
    
    public readonly int Width = width;
    public readonly int Height = height;
    public readonly List<RectangleF> Walls = walls;
    
    /// <summary>
    /// Создает игровую карту
    /// </summary>
    /// <param name="mapWidth">Длина карты</param>
    /// <param name="mapHeight">Ширина карты</param>
    /// <param name="wallOffset">Расстояние между стенами</param>
    /// <param name="wallMinSize">Минимальный размер стены (может не соблюдаться при выходе за границы карты)</param>
    /// <param name="wallMaxSize">Максимальный размер стены</param>
    /// <returns>Игровая карта</returns>
    public static Map Generate(int mapWidth,
        int mapHeight,
        int wallOffset,
        int wallMinSize,
        int wallMaxSize)
    {
        var walls = new List<RectangleF>();
        
        var cols = (int)Math.Ceiling((double)(mapWidth + wallOffset) / (wallMaxSize + wallOffset));
        var rows = (int)Math.Ceiling((double)(mapHeight + wallOffset) / (wallMaxSize + wallOffset));
        
        var totalWallWidth = cols * wallMaxSize + (cols - 1) * wallOffset;
        var totalWallHeight = rows * wallMaxSize + (rows - 1) * wallOffset;
        var startOffsetX = Math.Max(0, (mapWidth - totalWallWidth) / 2);
        var startOffsetY = Math.Max(0, (mapHeight - totalWallHeight) / 2);

        for (var col = 0; col < cols; col++)
            for (var row = 0; row < rows; row++)
            {
                var wall = GetWall(col,
                    row,
                    startOffsetX,
                    startOffsetY,
                    wallOffset,
                    wallMinSize,
                    wallMaxSize);
                var normalizedWall = NormalizeWall(mapWidth, mapHeight, wall);
                
                if (normalizedWall == null)
                    continue;
                
                walls.Add(normalizedWall.Value);
                Logger.LogDebug("Стена: {wall}", normalizedWall);
            }

        return new Map(mapWidth, mapHeight, walls);
    }
    
    /// <summary>
    /// Генерирует стену
    /// </summary>
    /// <param name="col">Условная колонка, в которой находится стена (иговая карта разбивается на колонки)</param>
    /// <param name="row">Условная строка, в которой находится стена (иговая карта разбивается на строки)</param>
    /// <param name="startOffsetX">Смещение стены по X</param>
    /// <param name="startOffsetY">Смещение стены по Y</param>
    /// <param name="offset">Расстояние между стенами</param>
    /// <param name="minSize">Минимальный размер стены</param>
    /// <param name="maxSize">Максимальный размер стены</param>
    /// <returns>Сгенерированная стена</returns>
    private static RectangleF GetWall(int col,
        int row,
        int startOffsetX,
        int startOffsetY,
        int offset,
        int minSize,
        int maxSize)
    {
        var startX = startOffsetX + col * (maxSize + offset);
        var startY = startOffsetY + row * (maxSize + offset);
        var wallSize = Random.Next(minSize, maxSize + 1);

        int endX;
        int endY;
                
        if (Random.Next(0, 2) == 0)
        {
            endX = startX + maxSize / 2;
            endY = startY + wallSize;
            startX = endX - 1;
        }
        else
        {
            endX = startX + wallSize;
            endY = startY + maxSize / 2;
            startY = endY - 1;
        }

        return RectangleF.FromLTRB(startX, startY, endX, endY);
    }
    
    /// <summary>
    /// Нормализует стену
    /// </summary>
    /// <param name="mapWidth">Длина карты</param>
    /// <param name="mapHeight">Ширина карты</param>
    /// <param name="wall">Стена</param>
    /// <returns>Нормализованая стена или null, если не удалось нормализовать</returns>
    private static RectangleF? NormalizeWall(int mapWidth, int mapHeight, RectangleF wall)
    {
        if (wall.Left >= mapWidth || wall.Top >= mapHeight)
            return null;
        
        var right = Math.Min(wall.Right, mapWidth);
        var bottom = Math.Min(wall.Bottom, mapHeight);
        
        return RectangleF.FromLTRB(wall.Left, wall.Top, right, bottom);
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

        foreach (var wall in Walls.Where(wall => wall.IntersectsWith(box)))
            return wall;

        return null;
    }
    
    /// <summary>
    /// Ищет пересечение точки со стенами или выход за пределы игровой карты
    /// </summary>
    /// <param name="point">Точка</param>
    /// <returns>Стена или граница, с которой пересекается точка (если пересечение существует)</returns>
    public RectangleF? FindIntersect(PointF point) => FindIntersect(new RectangleF(point.X, point.Y, 0, 0));
}