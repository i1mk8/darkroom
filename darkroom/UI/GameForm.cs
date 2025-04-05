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
        foreach (var wall in _game.Map.Walls) 
            PaintRectangle(wall, graphics, Brushes.Black);
    }

    private void PaintPlayer(Graphics graphics)
    {
        PaintRectangle(_game.Player.Box, graphics, Brushes.DodgerBlue);
    }

    private void PaintRectangle(RectangleF rectangle, Graphics graphics, Brush color)
    {
        graphics.FillRectangle(color, RectangleF.FromLTRB(rectangle.Left * _ratioX,
            rectangle.Top * _ratioY,
            rectangle.Right * _ratioX,
            rectangle.Bottom * _ratioY));
    }
}