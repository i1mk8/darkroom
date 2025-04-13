using darkroom.model;
using darkroom.UI.KeyEvent;
using Timer = System.Windows.Forms.Timer;

namespace darkroom.UI.form;

public sealed partial class GameForm : Form
{
    private const int FormWidth = 800;
    private const int FormHeight = 800;
    
    private readonly int _ratioX;
    private readonly int _ratioY;

    private readonly KeyEvent.KeyEvent _keyEvent;
    private readonly SingleKeyEvent _singleKeyEvent;
    
    private readonly List<PlayerWrapper> _wrappedPlayers;
    
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

        _keyEvent = new KeyEvent.KeyEvent(_game.MainPlayer);
        _singleKeyEvent = new SingleKeyEvent(_game.MainPlayer);

        KeyDown += _keyEvent.KeyDown;
        KeyUp += _keyEvent.KeyUp;
        KeyDown += _singleKeyEvent.KeyDown;
        KeyUp += _singleKeyEvent.KeyUp;
        
        _wrappedPlayers = PlayerWrapper.Wrap(_game.Players);
        
        InitializeComponent();
        InitializeTimer(60);
    }

    private void InitializeTimer(int fps)
    {
        var timer = new Timer();
        timer.Interval = 1000 / fps;
        timer.Tick += (_, _) =>
        {
            _game.Tick();
            _keyEvent.Proceed();
            Invalidate();
        };
        timer.Start();
    }

    private void PaintMap(Graphics graphics)
    {
        graphics.FillRectangle(Colors.BackgroundFill, RectangleF.FromLTRB(0, 0, FormWidth, FormHeight));
        foreach (var wall in _game.Map.Walls)
        {
            var wallBox = ResizeRectangle(wall);
            graphics.DrawRectangle(Colors.Wall, wallBox.X, wallBox.Y, wallBox.Width, wallBox.Height);
            graphics.FillRectangle(Colors.WallFill, wallBox);
        }
    }

    private void PaintPlayers(Graphics graphics)
    {
        foreach (var player in _wrappedPlayers)
            graphics.FillRectangle(player.Color, ResizeRectangle(player.Player.Box));
    }

    private void PaintFov(Graphics graphics)
    {
        var fov = _game.MainPlayer.Fov.GetFov();
        if (fov.Vertices.Count < 3)
            return;

        var vertices = fov.Vertices.Select(p => new PointF(p.X * _ratioX, p.Y * _ratioY)).ToArray();
    
        graphics.FillPolygon(Colors.PlayerFovFill, vertices);
        graphics.DrawPolygon(Colors.PlayerFov, vertices);
    }
    private RectangleF ResizeRectangle(RectangleF rectangle)
    {
        return RectangleF.FromLTRB(rectangle.Left * _ratioX,
            rectangle.Top * _ratioY,
            rectangle.Right * _ratioX,
            rectangle.Bottom * _ratioY);
    }
}