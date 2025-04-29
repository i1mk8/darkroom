using System.Drawing.Drawing2D;

namespace darkroom.UI.form;

public static class Colors
{
    public static readonly Brush BackgroundFill = new SolidBrush(ColorTranslator.FromHtml("#1E2124")); 
    
    public static readonly Brush WallFill = new HatchBrush(HatchStyle.LightUpwardDiagonal,
        ColorTranslator.FromHtml("#4A4C51"),
        Color.Transparent);
    public static readonly Pen Wall = new(ColorTranslator.FromHtml("#4A4C51"), 1.2f);
    
    public static readonly PlayerColor PlayerBlue = new(new SolidBrush(ColorTranslator.FromHtml("#6495ED")),
        "Синий");
    public static readonly PlayerColor PlayerGreen = new(new SolidBrush(ColorTranslator.FromHtml("#4DE818")),
        "Зелёный");
    public static readonly PlayerColor PlayerRed = new(new SolidBrush(ColorTranslator.FromHtml("#E81831")),
        "Красный");
    public static readonly PlayerColor PlayerPurple = new(new SolidBrush(ColorTranslator.FromHtml("#A918E8")),
        "Фиолетовый");
    public static readonly PlayerColor PlayerYellow = new(new SolidBrush(ColorTranslator.FromHtml("#FBE100")),
        "Жёлтый");

    public static readonly Brush FovFill = new SolidBrush(
        Color.FromArgb(30, ColorTranslator.FromHtml("#FFFFFF")));
    public static readonly Pen Fov = new(ColorTranslator.FromHtml("#FFFFFF"), 1.2f)
    {
        LineJoin = LineJoin.Round
    };
}

public class PlayerColor(Brush color, string colorName)
{
    public readonly Brush Color = color;
    public readonly string ColorName = colorName;
}