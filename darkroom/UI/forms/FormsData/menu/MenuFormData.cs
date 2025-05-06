using System.Drawing.Text;
using darkroom.UI.forms.FormsData.game;
using darkroom.UI.resources;
using darkroom.UI.sound;

namespace darkroom.UI.forms.FormsData.menu;

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
    
    private readonly KeyEvent _keyEvent;
    public KeyEvent keyEvent => _keyEvent;

    public MenuFormData(string title, Menu menu)
    {
        _title = title;
        Menu = menu;
        
        _scale = new Scale(OriginalWidth, OriginalHeight);
        _startY = GetStartY();
        
        InitializeFonts(out _titleFont, out _font);
        _keyEvent = new MenuController(this).GetKeyEvent();
    }
    
    public static MenuFormData GetMainMenu()
    {
        var menuItems = new List<MenuItem>
        {
            new("ОБУЧЕНИЕ", () => MainForm.MainForm.GetInstance().ShowData(new TrainingFormData())),
            new("ИГРАТЬ", () => MainForm.MainForm.GetInstance().ShowData(new GameFormData())),
            new("ВЫХОД", Application.Exit)
        };
        return new MenuFormData("DARKROOM", new Menu(menuItems));
    }
    
    private int GetStartY()
    {
        var spacing = _scale.ScaleY(OriginalLineSpacing);
        var font = _scale.ScaleNum(OriginalFontSize);
        var height = _scale.ScaleNum(OriginalTitleFontSize) + Menu.Items.Sum(_ => font + spacing);
        return (Screen.PrimaryScreen.Bounds.Height - height) / 2;
    }
    
    private void InitializeFonts(out Font titleFont, out Font font)
    {
        var fontCollection = new PrivateFontCollection();
        fontCollection.AddFontFile(Resources.PixelizerFontPath);
        
        var titleFontSize = _scale.ScaleNum(OriginalTitleFontSize);
        titleFont = new Font(fontCollection.Families[0], titleFontSize);
        
        var fontSize = _scale.ScaleNum(OriginalFontSize);
        font = new Font(fontCollection.Families[0], fontSize);
    }

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