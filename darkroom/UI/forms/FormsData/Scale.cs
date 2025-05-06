using darkroom.utils;

namespace darkroom.UI.forms.FormsData;

/// <summary>
/// Маштабирует (переносит в другое разрешение, сохраняя маштаб)
/// </summary>
public class Scale
{
    private readonly int _ratioX;
    private readonly int _ratioY;
    private readonly int _ratio;
    
    /// <param name="originalWidth">Исходная длина полотна</param>
    /// <param name="originalHeight">Исходная ширина полотна</param>
    public Scale(int originalWidth, int originalHeight)
    {
        _ratioX = Screen.PrimaryScreen.Bounds.Width / originalWidth;
        _ratioY = Screen.PrimaryScreen.Bounds.Height / originalHeight;
        _ratio = (_ratioX + _ratioY) / 2;
    }
    
    /// <summary>
    /// Маштабирует по X (по длине)
    /// </summary>
    /// <param name="x">Значение для маштабирования</param>
    /// <returns>Знаечние в новом маштабе</returns>
    public int ScaleX(int x) => x * _ratioX;
    
    /// <summary>
    /// Маштабирует по Y (по ширине)
    /// </summary>
    /// <param name="y">Значение для маштабирования</param>
    /// <returns>Знаечние в новом маштабе</returns>
    public int ScaleY(int y) => y * _ratioY;
    
    /// <summary>
    /// Маштабирует по X и Y (по длине и ширине)
    /// </summary>
    /// <param name="num">Значение для маштабирования</param>
    /// <returns>Знаечние в новом маштабе</returns>
    public int ScaleNum(int num) => num * _ratio;
    
    /// <summary>
    /// Маштабирует прямоугольник
    /// </summary>
    /// <param name="rectangle">Прямоугольник для маштабирования</param>
    /// <returns>Прямоугольник в новом маштабе</returns>
    public RectangleF ScaleRectangle(RectangleF rectangle) => 
        RectangleF.FromLTRB(rectangle.Left * _ratioX,
        rectangle.Top * _ratioY,
        rectangle.Right * _ratioX,
        rectangle.Bottom * _ratioY);
    
    /// <summary>
    /// Маштабирует полигон
    /// </summary>
    /// <param name="polygon">Полигон для маштабирования</param>
    /// <returns>Полигон в новом маштабе</returns>
    public Polygon ScalePolygon(Polygon polygon) => new Polygon(polygon.Vertices.Select(verticle=>
        new PointF(verticle.X * _ratioX, verticle.Y * _ratioY)).ToList());
}