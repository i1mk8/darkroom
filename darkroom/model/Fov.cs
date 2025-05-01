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
/// <param name="angleOffset">Смещение угла, используемое при расчете Fov, влияет на количество лучей</param>
/// <param name="distanceOffset">Смещение дистанции, используемое при расчеете длины луча</param>
public class Fov(Map map,
    Player player, float viewDistance,
    float viewAngle,
    float baseAngleSpeed,
    float angleOffset,
    float distanceOffset)
{
    public float BaseAngle; // Угол, характеризующий направление взгляда
    
    /// <summary>
    /// Поворачивает взгляд направо
    /// </summary>
    public void MoveRight() => SetBaseAngle(BaseAngle + baseAngleSpeed);
    /// <summary>
    /// Поворачивает взгляд налево
    /// </summary>
    public void MoveLeft() => SetBaseAngle(BaseAngle - baseAngleSpeed);
    
    /// <summary>
    /// Получение FOV игрока
    /// </summary>
    /// <returns>Полигон FOV</returns>
    public Polygon GetFov()
    {
        var origin = player.Box.Center();
        var polygonVertices = new List<PointF> { origin };

        for (var angle = BaseAngle - viewAngle / 2; angle <= BaseAngle + viewAngle / 2; angle += angleOffset)
            polygonVertices.Add(GetRayEndPoint(Utils.ToRadians(angle), origin.X, origin.Y));
        
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
        angle %= 360;
        BaseAngle = angle < 0 ? angle + 360 : angle;
    }
}