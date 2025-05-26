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
    private const int OriginalMenuItemFontSize = 100;
    private const int OriginalHintFontSize = 25;
    
    private const int OriginalLineSpacing = 60;
    private const int OriginalHintLineSpacing = 40;
    private const int OriginalHintOffset = 60;
    
    private readonly Scale _scale;
    private readonly int _startY;
    
    private readonly PrivateFontCollection _fontCollection = new();
    private readonly Font _titleFont;
    private readonly Font _menuItemFont;
    private readonly Font _hintFont;
    
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
        
        InitializeFonts(out _titleFont, out _menuItemFont, out _hintFont);
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
        var font = _scale.ScaleNum(OriginalMenuItemFontSize);
        var height = _scale.ScaleNum(OriginalTitleFontSize) + Menu.Items.Sum(_ => font + spacing);
        return (Screen.PrimaryScreen.Bounds.Height - height) / 2;
    }
    
    /// <summary>
    /// Инициализирует шрифты
    /// </summary>
    /// <param name="titleFont">Шрифт загловка</param>
    /// <param name="menuItemFont">Шрифт эелементов меню</param>
    /// <param name="hintFont">Шрифт подксказки</param>
    private void InitializeFonts(out Font titleFont, out Font menuItemFont, out Font hintFont)
    {
        _fontCollection.AddFontFile(Resources.PixelizerFontPath);
        
        var titleFontSize = _scale.ScaleNum(OriginalTitleFontSize);
        titleFont = new Font(_fontCollection.Families[0], titleFontSize);
        
        var menuItemFontSize = _scale.ScaleNum(OriginalMenuItemFontSize);
        menuItemFont = new Font(_fontCollection.Families[0], menuItemFontSize);
        
        var hintFontSize = _scale.ScaleNum(OriginalHintFontSize);
        hintFont = new Font(_fontCollection.Families[0], hintFontSize);
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
        var fontSize = _scale.ScaleNum(OriginalMenuItemFontSize);
        
        var y = _startY + _scale.ScaleNum(OriginalTitleFontSize) + spacing;
        foreach (var menuItem in Menu.Items)
        {
            var brush = Colors.UnselectedFontBrush;
            if (menuItem.Selected)
                brush = Colors.FontBrush;
            
            var size = graphics.MeasureString(menuItem.Text, _menuItemFont);
            graphics.DrawString(menuItem.Text, 
                _menuItemFont,
                brush, 
                Screen.PrimaryScreen.Bounds.Width / 2 - size.Width / 2,
                y);
            y += fontSize + spacing;
        }
    }

    private void PaintHint(Graphics graphics)
    {
        var hints = new List<string>
        {
            "СТРЕЛОЧКИ ВВЕРХ/ВНИЗ ДЛЯ ПЕРЕМЕЩЕНИЯ ПО МЕНЮ",
            "ENTER ДЛЯ ВЫБОРА"
        };
        
        var fontSize = _scale.ScaleNum(OriginalHintFontSize);
        var spacing = _scale.ScaleY(OriginalHintLineSpacing);
        var offset = _scale.ScaleNum(OriginalHintOffset);

        var y = Screen.PrimaryScreen.Bounds.Height - (hints.Sum(_ => fontSize + spacing) - spacing + offset);
        foreach (var hint in hints)
        {
            var size = graphics.MeasureString(hint, _hintFont);
            graphics.DrawString(hint, 
                _hintFont,
                Colors.FontBrush, 
                Screen.PrimaryScreen.Bounds.Width / 2 - size.Width / 2,
                y);
            y += fontSize + spacing;
        }
    }
    
    public void OnTimerTick() { }

    public void OnPaint(Graphics graphics)
    {
        PaintBackground(graphics);
        PaintMenu(graphics);
        PaintHint(graphics);
    }
}