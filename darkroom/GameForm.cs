using darkroom.model;
using Timer = System.Windows.Forms.Timer;

namespace darkroom;

public sealed partial class GameForm : Form
{
    private const int FormWidth = 800;
    private const int FormHeight = 800;
    
    private readonly Game _game;
    
    private readonly int _ratioX;
    private readonly int _ratioY;
    
    public GameForm()
    {
        _game = new Game();
        
        _ratioX = FormWidth / _game.Map.Width;
        _ratioY = FormHeight / _game.Map.Height;
        
        InitializeComponent();
        DoubleBuffered = true;
        InitializeTimer(60);
    }

    private void InitializeTimer(int fps)
    {
        const double fpsCoefficient = 3.75;
        
        var timer = new Timer();
        timer.Interval = (int)(fps / fpsCoefficient);
        timer.Tick += (_, _) =>
        {
            Invalidate();
        };
        timer.Start();
    }

    private void PaintMap(Graphics graphics)
    {
        foreach (var wall in _game.Map.Walls) 
            PaintRectangle(wall, graphics, Brushes.Black);
    }

    private void PaintPlayers(Graphics graphics)
    {
        foreach (var player in _game.Players) 
            PaintRectangle(player.Box, graphics, Brushes.DodgerBlue);
    }

    private void PaintRectangle(RectangleF rectangle, Graphics graphics, Brush color)
    {
        graphics.FillRectangle(color, RectangleF.FromLTRB(rectangle.Left * _ratioX,
            rectangle.Top * _ratioY,
            rectangle.Right * _ratioX,
            rectangle.Bottom * _ratioY));
    }
}