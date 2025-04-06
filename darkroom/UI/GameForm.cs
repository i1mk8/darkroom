using darkroom.model;
using Timer = System.Windows.Forms.Timer;

namespace darkroom.UI;

public sealed partial class GameForm : Form
{
    private const int FormWidth = 800;
    private const int FormHeight = 800;
    
    private readonly int _ratioX;
    private readonly int _ratioY;

    private readonly KeyEvent _keyEvent;
    
    private readonly Game _game;
    
    public GameForm()
    {
        DoubleBuffered = true;
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
        PaintRectangle(RectangleF.FromLTRB(0, 0, FormWidth, FormHeight), graphics, Brushes.Black);
        foreach (var wall in _game.Map.Walls) 
            PaintRectangle(wall, graphics, Brushes.Gray);
    }

    private void PaintPlayer(Graphics graphics)
    {
        PaintRectangle(_game.Player.Box, graphics, Brushes.DodgerBlue);
    }

    private void PaintFov(Graphics graphics)
    {
        var fov = _game.Player.Fov.GetFov();
        if (fov.Vertices.Count < 3)
            return;
        
        var path = new System.Drawing.Drawing2D.GraphicsPath();
        var vertices = fov.Vertices.Select(p => new PointF(p.X * _ratioX, p.Y * _ratioY)).ToArray();

        path.AddPolygon(vertices);
        graphics.FillPath(Brushes.White, path);
    }

    private void PaintRectangle(RectangleF rectangle, Graphics graphics, Brush color)
    {
        graphics.FillRectangle(color, RectangleF.FromLTRB(rectangle.Left * _ratioX,
            rectangle.Top * _ratioY,
            rectangle.Right * _ratioX,
            rectangle.Bottom * _ratioY));
    }
}