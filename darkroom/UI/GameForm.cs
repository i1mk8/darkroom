using System.Drawing.Drawing2D;
using darkroom.model;
using Timer = System.Windows.Forms.Timer;

namespace darkroom.UI;

public sealed partial class GameForm : Form
{
    private const int FormWidth = 800;
    private const int FormHeight = 800;
    
    private readonly int _ratioX;
    private readonly int _ratioY;
    
    private readonly Brush _backgroundFillColor = new SolidBrush(ColorTranslator.FromHtml("#1E2124"));
    
    private readonly Brush _wallFillColor = new HatchBrush(
        HatchStyle.LightUpwardDiagonal, 
        ColorTranslator.FromHtml("#4A4C51"), 
        Color.Transparent
    );
    private readonly Pen _wallColor = new(ColorTranslator.FromHtml("#4A4C51"), 1.2f);
    
    private readonly Brush _playerFillColor = new SolidBrush(ColorTranslator.FromHtml("#6495ED"));
    
    private readonly Brush _playerFovFillColor = new HatchBrush(
        HatchStyle.LightUpwardDiagonal,
        Color.FromArgb(80, ColorTranslator.FromHtml("#FFFFFF")),
        Color.Transparent
    );
    private readonly Pen _playerFovColor = new(ColorTranslator.FromHtml("#FFFFFF"), 1.2f){
        LineJoin = LineJoin.Round
    };

    private readonly KeyEvent _keyEvent;
    
    private readonly Game _game;
    
    public GameForm()
    {
        SetStyle(ControlStyles.OptimizedDoubleBuffer
                 | ControlStyles.AllPaintingInWmPaint
                 |ControlStyles.UserPaint, true);
        KeyPreview = true;
        
        _game = new Game();
        
        _ratioX = FormWidth / _game.Map.Width;
        _ratioY = FormHeight / _game.Map.Height;

        _keyEvent = new KeyEvent(_game.Player);
        
        InitializeComponent();

        KeyDown += _keyEvent.KeyDown;
        KeyUp += _keyEvent.KeyUp;
        
        InitializeTimer(60);
    }

    private void InitializeTimer(int fps)
    {
        var timer = new Timer();
        timer.Interval = 1000 / fps;
        timer.Tick += (_, _) =>
        {
            _keyEvent.ProceedMovement();
            Invalidate();
        };
        timer.Start();
    }

    private void PaintMap(Graphics graphics)
    {
        graphics.FillRectangle(_backgroundFillColor, RectangleF.FromLTRB(0, 0, FormWidth, FormHeight));
        foreach (var wall in _game.Map.Walls)
        {
            var wallBox = ResizeRectangle(wall);
            graphics.DrawRectangle(_wallColor, wallBox.X, wallBox.Y, wallBox.Width, wallBox.Height);
            graphics.FillRectangle(_wallFillColor, wallBox);
        }
    }

    private void PaintPlayer(Graphics graphics)
    {
        graphics.FillRectangle(_playerFillColor, ResizeRectangle(_game.Player.Box));
    }

    private void PaintFov(Graphics graphics)
    {
        var fov = _game.Player.Fov.GetFov();
        if (fov.Vertices.Count < 3)
            return;

        var vertices = fov.Vertices.Select(p => new PointF(p.X * _ratioX, p.Y * _ratioY)).ToArray();
    
        graphics.FillPolygon(_playerFovFillColor, vertices);
        graphics.DrawPolygon(_playerFovColor, vertices);
    }
    private RectangleF ResizeRectangle(RectangleF rectangle)
    {
        return RectangleF.FromLTRB(rectangle.Left * _ratioX,
            rectangle.Top * _ratioY,
            rectangle.Right * _ratioX,
            rectangle.Bottom * _ratioY);
    }
}