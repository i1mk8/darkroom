using darkroom.utils;

namespace darkroom.model;

/// <summary>
/// Область видимости игрока
/// </summary>
/// <param name="map">Игровая карта</param>
/// <param name="player">Игрок, за которым закреплен FOV</param>
/// <param name="viewDistance">Дальность видимости</param>
/// <param name="viewAngle">Угол обзора</param>
/// <param name="baseAngleSpeed">Скорость поворота направления взгляда</param>
public class Fov(Map map, Player player, float viewDistance, float viewAngle, float baseAngleSpeed)
{
    private float _baseAngle; // Угол, характеризующий направление взгляда
    
    /// <summary>
    /// Поворачивает взгляд направо
    /// </summary>
    public void MoveRight() => SetBaseAngle(_baseAngle + baseAngleSpeed);
    /// <summary>
    /// Поворачивает взгляд налево
    /// </summary>
    public void MoveLeft() => SetBaseAngle(_baseAngle - baseAngleSpeed);
    
    /// <summary>
    /// Получение FOV игрока
    /// </summary>
    /// <returns>Полигон FOV</returns>
    public Polygon GetFov()
    {
        var polygonVertices = new List<PointF>();
        
        var originX = player.Box.X + player.Box.Width / 2;
        var originY = player.Box.Y + player.Box.Height / 2;
        
        var origin = new PointF(originX, originY);
        polygonVertices.Add(origin);

        const float angleOffset = 0.5f;
        for (var angle = _baseAngle - viewAngle / 2; angle <= _baseAngle + viewAngle / 2; angle += angleOffset)
            polygonVertices.Add(GetRayEndPoint(angle * MathF.PI / 180, originX, originY));
        
        polygonVertices.Add(origin);
        return new Polygon(polygonVertices);
    }
    
    /// <summary>
    /// Получение конечной точки луча
    /// </summary>
    /// <param name="angle">Угол отклонения от начальной позиции</param>
    /// <param name="originX">Исходящая координата по X</param>
    /// <param name="originY">Исходящая координата по Y</param>
    /// <returns>Конечная точка луча</returns>
    private PointF GetRayEndPoint(float angle, float originX, float originY)
    {
        var direction = new PointF(MathF.Cos(angle), MathF.Sin(angle));
        const float distanceOffset = 0.05f;
        
        for (float distance = 0; distance < viewDistance; distance += distanceOffset)
        {
            var point = new PointF(originX + direction.X * distance, originY + direction.Y * distance);
            if (map.FindIntersect(point) != null)
                return point;
        }
        
        return new PointF(originX + direction.X * viewDistance, originY + direction.Y * viewDistance);
    }
    
    /// <summary>
    /// Устанавливает и нормализует угол направления взгляда
    /// </summary>
    /// <param name="angle">Устанавлевамое значение</param>
    private void SetBaseAngle(float angle)
    {
        Console.WriteLine(angle);
        if (angle > 360)
            angle -= 360 * (int)(angle / 360);
        else if (angle < -360)
            angle += 360 * (int)(angle / -360);
        _baseAngle = angle;
    }
}