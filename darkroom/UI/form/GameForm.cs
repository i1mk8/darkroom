using darkroom.model;
using darkroom.UI.KeyEvent;
using darkroom.utils;
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
    
    private readonly List<BotWrapper> _wrappedPlayers;
    
    private bool _debug;
    
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
        _singleKeyEvent = new SingleKeyEvent(_game.MainPlayer, ToggleDebug);

        KeyDown += _keyEvent.KeyDown;
        KeyUp += _keyEvent.KeyUp;
        KeyDown += _singleKeyEvent.KeyDown;
        KeyUp += _singleKeyEvent.KeyUp;

        _wrappedPlayers = BotWrapper.Wrap(_game.Bots);
        
        InitializeComponent();
        InitializeTimer(60);
    }

    private void InitializeTimer(int fps)
    {
        var timer = new Timer();
        var interval = 1000 / fps;
        timer.Interval = interval;
        
        var stopWatch = new System.Diagnostics.Stopwatch();
        var fpsCounter = new FpsCounter();

        timer.Tick += (_, _) =>
        {
            stopWatch.Restart();

            OnTimerTick();

            stopWatch.Stop();
            var nextInterval = (int)Math.Max(1, interval - stopWatch.ElapsedMilliseconds);
            
            var currentFps = fpsCounter.Update();
            if (currentFps != null)
                Console.WriteLine($"Current Fps: {currentFps}");
            
            timer.Interval = nextInterval;
        };
    
        timer.Start();
    }

    private void OnTimerTick()
    {
        _game.Tick();
        _keyEvent.Proceed();
        Invalidate();
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

    private void PaintPlayers(Graphics graphics, Polygon mainPlayerFov)
    {
        graphics.FillRectangle(Colors.PlayerFillBlue, ResizeRectangle(_game.MainPlayer.Box));
        foreach (var bot in _wrappedPlayers.Where(player => _debug || mainPlayerFov.Contains(player.Bot.Box)))
        {
            if (_debug)
            {
                var botFov = bot.Bot.Fov.GetFov();
                if (botFov.Vertices.Count >= 3)
                    PaintFov(graphics, botFov);
            }
            
            graphics.FillRectangle(bot.Color, ResizeRectangle(bot.Bot.Box));
        }
    }

    private void PaintFov(Graphics graphics, Polygon fov)
    {
        var vertices = fov.Vertices.Select(p => new PointF(p.X * _ratioX, p.Y * _ratioY)).ToArray();
    
        graphics.FillPolygon(Colors.FovFill, vertices);
        graphics.DrawPolygon(Colors.Fov, vertices);
    }
    private RectangleF ResizeRectangle(RectangleF rectangle)
    {
        return RectangleF.FromLTRB(rectangle.Left * _ratioX,
            rectangle.Top * _ratioY,
            rectangle.Right * _ratioX,
            rectangle.Bottom * _ratioY);
    }
    
    private void ToggleDebug() => _debug = !_debug;
}