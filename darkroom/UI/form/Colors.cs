using System.Drawing.Drawing2D;

namespace darkroom.UI.form;

public static class Colors
{
    public static readonly Brush BackgroundFill = new SolidBrush(ColorTranslator.FromHtml("#1E2124")); 
    
    public static readonly Brush WallFill = new HatchBrush(HatchStyle.LightUpwardDiagonal,
        ColorTranslator.FromHtml("#4A4C51"),
        Color.Transparent);
    public static readonly Pen Wall = new(ColorTranslator.FromHtml("#4A4C51"), 1.2f);
    
    public static readonly Brush PlayerFillBlue = new SolidBrush(ColorTranslator.FromHtml("#6495ED"));
    public static readonly Brush PlayerFillGreen = new SolidBrush(ColorTranslator.FromHtml("#4DE818"));
    public static readonly Brush PlayerFillRed = new SolidBrush(ColorTranslator.FromHtml("#E81831"));
    public static readonly Brush PlayerFillPurple = new SolidBrush(ColorTranslator.FromHtml("#A918E8"));
    public static readonly Brush PlayerFillYellow = new SolidBrush(ColorTranslator.FromHtml("#FBE100"));

    public static readonly Brush FovFill = new SolidBrush(
        Color.FromArgb(30, ColorTranslator.FromHtml("#FFFFFF")));
    public static readonly Pen Fov = new(ColorTranslator.FromHtml("#FFFFFF"), 1.2f)
    {
        LineJoin = LineJoin.Round
    };
}