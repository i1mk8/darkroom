using System.Drawing.Drawing2D;

namespace darkroom.UI.forms.FormsData.game;

/// <summary>
/// Цвета
/// </summary>
public static class Colors
{
    public static readonly Brush BackgroundBrush = new SolidBrush(ColorTranslator.FromHtml("#1E2124")); 
    
    public static readonly Brush WallBrush = new HatchBrush(HatchStyle.LightUpwardDiagonal,
        ColorTranslator.FromHtml("#4A4C51"),
        Color.Transparent);
    public static readonly Pen WallPen = new(ColorTranslator.FromHtml("#4A4C51"), 1.2f);
    
    public static readonly PlayerColor PlayerBlue = new("#6495ED","Синий");
    public static readonly PlayerColor PlayerGreen = new("#4DE818","Зелёный");
    public static readonly PlayerColor PlayerRed = new("#E81831", "Красный");
    public static readonly PlayerColor PlayerPurple = new("#A918E8", "Фиолетовый");
    public static readonly PlayerColor PlayerYellow = new("#FBE100", "Жёлтый");

    public static readonly Brush FovBrush = new SolidBrush(Color.FromArgb(30,
        ColorTranslator.FromHtml("#FFFFFF")));
    public static readonly Pen FovPen = new(ColorTranslator.FromHtml("#FFFFFF"), 1.2f)
    {
        LineJoin = LineJoin.Round
    };
}

/// <summary>
/// Цвет игрока с заданным hex кодом и названием
/// </summary>
public class PlayerColor
{
    public readonly Brush Brush; 
    public readonly Pen Pen;
    public readonly string ColorName;
    
    /// <param name="color">Hex код цвета</param>
    /// <param name="colorName">Название цвета</param>
    public PlayerColor(string color, string colorName)
    {
        var colorTranslator = ColorTranslator.FromHtml(color);
        Brush = new SolidBrush(colorTranslator);
        Pen = new Pen(colorTranslator);
        ColorName = colorName;
    }
}