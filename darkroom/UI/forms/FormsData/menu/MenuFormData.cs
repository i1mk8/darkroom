using System.Drawing.Text;
using darkroom.UI.forms.FormsData.KeyEvent;
using darkroom.UI.resources;

namespace darkroom.UI.forms.FormsData.menu;

/// <summary>
/// Универсальный пользовательский интерйефс меню
/// </summary>
public class MenuFormData : IFormData
{
    private const int OriginalWidth = 1920;
    private const int OriginalHeight = 1080;

    private const int OriginalTitleFontSize = 200;
    private const int OriginalFontSize = 100;
    private const int OriginalLineSpacing = 60;
    
    private readonly Scale _scale;
    private readonly int _startY;
    
    private readonly Font _titleFont;
    private readonly Font _font;
    
    private readonly string _title;
    public readonly Menu Menu;
    
    public KeyEventController keyEventController { get; }
    
    /// <summary>
    /// Конструктор пользовательского интерфейса меню
    /// </summary>
    /// <param name="title">Заголовок</param>
    /// <param name="menu">Меню</param>
    public MenuFormData(string title, Menu menu)
    {
        _title = title;
        Menu = menu;
        
        _scale = new Scale(OriginalWidth, OriginalHeight);
        _startY = GetStartY();
        
        InitializeFonts(out _titleFont, out _font);
        keyEventController = new MenuController(this);
    }
    
    /// <summary>
    /// Возвращае стартувую координату по Y, с которой начинается отрисовка.
    /// Расчитывается таким образом, чтобы элементы интерфейса находились по центру
    /// </summary>
    /// <returns>Стартувая координата по Y, с которой начинается отрисовка</returns>
    private int GetStartY()
    {
        var spacing = _scale.ScaleY(OriginalLineSpacing);
        var font = _scale.ScaleNum(OriginalFontSize);
        var height = _scale.ScaleNum(OriginalTitleFontSize) + Menu.Items.Sum(_ => font + spacing);
        return (Screen.PrimaryScreen.Bounds.Height - height) / 2;
    }
    
    /// <summary>
    /// Инициализирует шрифты
    /// </summary>
    /// <param name="titleFont">Шрифт загловка</param>
    /// <param name="font">Шрифт эелементов меню</param>
    private void InitializeFonts(out Font titleFont, out Font font)
    {
        var fontCollection = new PrivateFontCollection();
        fontCollection.AddFontFile(Resources.PixelizerFontPath);
        
        var titleFontSize = _scale.ScaleNum(OriginalTitleFontSize);
        titleFont = new Font(fontCollection.Families[0], titleFontSize);
        
        var fontSize = _scale.ScaleNum(OriginalFontSize);
        font = new Font(fontCollection.Families[0], fontSize);
    }
    
    /// <summary>
    /// Рисует задний фон и заголовок
    /// </summary>
    /// <param name="graphics">Графика для рисования</param>
    private void PaintBackground(Graphics graphics)
    {
        graphics.FillRectangle(Colors.BackgroundBrush,
            0, 
            0,
            Screen.PrimaryScreen.Bounds.Width,
            Screen.PrimaryScreen.Bounds.Height);
        
        var size = graphics.MeasureString(_title, _titleFont);
        graphics.DrawString(_title, 
            _titleFont,
            Colors.FontBrush, 
            Screen.PrimaryScreen.Bounds.Width / 2 - size.Width / 2,
            _startY);
    }
    
    /// <summary>
    /// Рисует элементы меню
    /// </summary>
    /// <param name="graphics">Графика для рисования</param>
    private void PaintMenu(Graphics graphics)
    {
        var spacing = _scale.ScaleY(OriginalLineSpacing);
        var font = _scale.ScaleNum(OriginalFontSize);
        
        var y = _startY + _scale.ScaleNum(OriginalTitleFontSize) + spacing;
        foreach (var menuItem in Menu.Items)
        {
            var brush = Colors.UnselectedFontBrush;
            if (menuItem.Selected)
                brush = Colors.FontBrush;
            
            var size = graphics.MeasureString(menuItem.Text, _font);
            graphics.DrawString(menuItem.Text, 
                _font,
                brush, 
                Screen.PrimaryScreen.Bounds.Width / 2 - size.Width / 2,
                y);
            y += font + spacing;
        }
    }
    
    public void OnTimerTick() { }

    public void OnPaint(Graphics graphics)
    {
        PaintBackground(graphics);
        PaintMenu(graphics);
    }
}