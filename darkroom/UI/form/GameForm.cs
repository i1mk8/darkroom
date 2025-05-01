using darkroom.model;
using darkroom.UI.KeyEvent;
using darkroom.utils;
using Timer = System.Windows.Forms.Timer;

namespace darkroom.UI.form;

/// <summary>
/// Форма, в которой отрисовывается игра
/// </summary>
public sealed partial class GameForm : Form
{
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
        
        _ratioX = Screen.PrimaryScreen.Bounds.Width / _game.Map.Width;
        _ratioY = Screen.PrimaryScreen.Bounds.Height / _game.Map.Height;

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
    
    /// <summary>
    /// Инициализирует таймер с заданным fps.
    /// </summary>
    /// <param name="fps">Fps (количество кадров в секунду)</param>
    private void InitializeTimer(int fps)
    {
        var timer = new Timer();
        var interval = 1000 / fps;
        timer.Interval = 1;
        
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
    
    /// <summary>
    /// Обрабатывает событие тика таймера: обновляет состояние игры, обрабатывает ввод и перерисовывает форму
    /// </summary>
    private void OnTimerTick()
    {
        _game.Tick();
        _keyEvent.Proceed();
        Invalidate();
    }

    /// <summary>
    /// Отрисовывает статистику игроков
    /// </summary>
    /// <param name="graphics">Графика для рисования</param>
    private void PaintStats(Graphics graphics)
    {
        var stats = new List<(PlayerColor color, int Kills)> { (Colors.PlayerBlue, _game.MainPlayer.KillsCount) };
        stats.AddRange(_wrappedPlayers.Select(bot => (bot.Color, bot.Bot.KillsCount)));
    
        var font = new Font("Arial", 15, FontStyle.Bold);
        var x = 20;
        var y = 20;
        const int xOffset = 15;
    
        foreach (var player in stats.OrderByDescending(p => p.Kills))
        {
            var text = $"{player.color.ColorName}: {player.Kills}";
            var textSize = graphics.MeasureString(text, font);
            graphics.DrawString(text, font, player.color.Brush, x, y);
            x += (int)textSize.Width + xOffset;
        }
    }

    /// <summary>
    /// Отрисовывает пули в поле зрения главного игрока
    /// </summary>
    /// <param name="graphics">Графика для рисования</param>
    /// <param name="mainPlayerFov">Поле зрения главного игрока</param>
    private void PaintBullets(Graphics graphics, Polygon mainPlayerFov)
    {
        foreach (var bullet in _game.BulletProcessor.Bullets.Where(bullet => mainPlayerFov.Contains(bullet.Box)))
            graphics.FillRectangle(Colors.BulletBrush, ResizeRectangle(bullet.Box));
    }

    /// <summary>
    /// Отрисовывает карту: стены и фон
    /// </summary>
    /// <param name="graphics">Графика для рисования</param>
    private void PaintMap(Graphics graphics)
    {
        graphics.FillRectangle(Colors.BackgroundBrush, RectangleF.FromLTRB(0, 
            0, 
            Screen.PrimaryScreen.Bounds.Width, 
            Screen.PrimaryScreen.Bounds.Height));
        foreach (var wall in _game.Map.Walls)
        {
            var wallBox = ResizeRectangle(wall);
            graphics.DrawRectangle(Colors.WallPen, wallBox.X, wallBox.Y, wallBox.Width, wallBox.Height);
            graphics.FillRectangle(Colors.WallBrush, wallBox);
        }
    }
    
    /// <summary>
    /// Отрисовывает игроков
    /// </summary>
    /// <param name="graphics">Графика для рисования</param>
    /// <param name="mainPlayerFov">Поле зрения главного игрока</param>
    private void PaintPlayers(Graphics graphics, Polygon mainPlayerFov)
    {
        graphics.FillRectangle(Colors.PlayerBlue.Brush, ResizeRectangle(_game.MainPlayer.Box));
        foreach (var bot in _wrappedPlayers.Where(player => _debug || mainPlayerFov.Contains(player.Bot.Box)))
        {
            if (_debug)
            {
                var botFov = bot.Bot.Fov.GetFov();
                if (botFov.Vertices.Count >= 3)
                    PaintFov(graphics, botFov, bot.Color.Pen);
            }
            
            graphics.FillRectangle(bot.Color.Brush, ResizeRectangle(bot.Bot.Box));
        }
    }
    
    /// <summary>
    /// Отрисовывает поле зрения игрока
    /// </summary>
    /// <param name="graphics">Графика для рисования</param>
    /// <param name="fov">Поле зрения</param>
    /// <param name="aimColor">Цвет линии прицела</param>
    private void PaintFov(Graphics graphics, Polygon fov, Pen aimColor)
    {
        var vertices = fov.Vertices.Select(p => new PointF(p.X * _ratioX, p.Y * _ratioY)).ToArray();
    
        graphics.FillPolygon(Colors.FovBrush, vertices);
        graphics.DrawPolygon(Colors.FovPen, vertices);
        graphics.DrawLine(aimColor, vertices[0], vertices[vertices.Length / 2]);
    }
    
    /// <summary>
    /// Масштабирует прямоугольник в соответствии с разрешением экрана
    /// </summary>
    /// <param name="rectangle">Прямоугольник в координатах игры</param>
    /// <returns>Прямоугольник в координатах экрана</returns>
    private RectangleF ResizeRectangle(RectangleF rectangle)
    {
        return RectangleF.FromLTRB(rectangle.Left * _ratioX,
            rectangle.Top * _ratioY,
            rectangle.Right * _ratioX,
            rectangle.Bottom * _ratioY);
    }
    
    /// <summary>
    /// Переключает режим отладки (постоянное отображение ботов)
    /// </summary>
    private void ToggleDebug() => _debug = !_debug;
}