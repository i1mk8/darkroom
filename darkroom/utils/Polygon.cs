namespace darkroom.utils;

/// <summary>
/// Геометрический полигон
/// </summary>
/// <param name="vertices">Вершины полигона</param>
public class Polygon(List<PointF> vertices)
{
    public readonly List<PointF> Vertices = vertices;
    
    /// <summary>
    /// Проверяет пересечение объекта с полигоном
    /// </summary>
    /// <param name="box">Бокс объекта</param>
    /// <returns></returns>
    public bool Contains(RectangleF box) {
        return Contains(box.Location)
               || Contains(new PointF(box.Right, box.Top))
               || Contains(new PointF(box.Right, box.Bottom))
               || Contains(new PointF(box.Left, box.Bottom)); 
    }
    
    /// <summary>
    /// Проверяет наличие точки внутри полигона
    /// </summary>
    /// <param name="point">Точка</param>
    /// <returns></returns>
    public bool Contains(PointF point)
    {
        var result = false;
        var previousVertex = Vertices[^1];

        foreach (var vertex in Vertices)
        {
            if ((vertex.Y < point.Y && previousVertex.Y >= point.Y||
                 previousVertex.Y < point.Y && vertex.Y >= point.Y)
                && vertex.X + (point.Y - vertex.Y) / (previousVertex.Y - vertex.Y) * (previousVertex.X - vertex.X) < point.X)
                result = !result;
            previousVertex = vertex;
        }
        
        return result;
    }
}