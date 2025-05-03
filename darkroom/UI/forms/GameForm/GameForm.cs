using darkroom.model;
using darkroom.UI.form;
using darkroom.utils;
using Microsoft.Extensions.Logging;
using Timer = System.Windows.Forms.Timer;

namespace darkroom.UI.forms.GameForm;

/// <summary>
/// Форма, в которой отрисовывается игра
/// </summary>
public sealed partial class GameForm : Form
{
    private readonly ILogger _logger = Utils.LoggerFactory.CreateLogger<GameForm>();
    
    private readonly int _ratioX;
    private readonly int _ratioY;
    
    private readonly Game _game;
    
    private readonly KeyEvent _keyEvent;
    private readonly List<BotWrapper> _wrappedBots;
    
    private bool _debug;
    
    public GameForm()
    {
        SetStyle(ControlStyles.OptimizedDoubleBuffer
                 | ControlStyles.AllPaintingInWmPaint
                 |ControlStyles.UserPaint, true);
        KeyPreview = true;
        
        _game = new Game();
        
        _ratioX = Screen.PrimaryScreen.Bounds.Width / _game.Map.Width;
        _ratioY = Screen.PrimaryScreen.Bounds.Height / _game.Map.Height;
        
        InitializeKeyEvent(out _keyEvent);

        _wrappedBots = BotWrapper.Wrap(_game.Bots);
        
        InitializeComponent();
        InitializeTimer();
    }
    
    /// <summary>
    /// Инициализирует обработчик нажатий
    /// </summary>
    /// <param name="keyEvent">Обработчик нажатий</param>
    private void InitializeKeyEvent(out KeyEvent keyEvent)
    {
        var keyEventsActions = new List<KeyEventAction>
        {
            new(Keys.W, false, _game.MainPlayer.MoveBack),
            new(Keys.A, false, _game.MainPlayer.MoveLeft),
            new(Keys.S, false, _game.MainPlayer.MoveForward),
            new(Keys.D, false, _game.MainPlayer.MoveRight),
            
            new(Keys.Right, false, _game.MainPlayer.Fov.MoveRight),
            new(Keys.Left, false, _game.MainPlayer.Fov.MoveLeft),
            
            new(Keys.Up, true, _game.MainPlayer.Shoot),
            new(Keys.Space, true, _game.MainPlayer.Shoot),
            new(Keys.Oemtilde, true, () => _debug = !_debug),
        };
        keyEvent = new KeyEvent(keyEventsActions);

        KeyDown += keyEvent.KeyDown;
        KeyUp += keyEvent.KeyUp;
    }
    
    /// <summary>
    /// Инициализирует таймер с заданным fps.
    /// </summary>
    private void InitializeTimer()
    {
        var timer = new Timer();
        timer.Interval = 1;
        
        var fpsCounter = new FpsCounter();
        timer.Tick += (_, _) =>
        {
            OnTimerTick();
            
            var currentFps = fpsCounter.Update();
            if (currentFps != null)
                _logger.LogInformation("Fps: {fps}", currentFps);
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
        stats.AddRange(_wrappedBots.Select(bot => (bot.Color, bot.Bot.KillsCount)));
    
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
        foreach (var bullet in _game.BulletProcessor.Bullets.Where(bullet =>
                     _debug || mainPlayerFov.Contains(bullet.Box)))
        {
            var color = Colors.PlayerBlue.Brush;
            if (bullet.Shooter != _game.MainPlayer)
                color = _wrappedBots.FirstOrDefault(bot => bot.Bot == bullet.Shooter)!.Color.Brush;
            
            graphics.FillRectangle(color, ResizeRectangle(bullet.Box));
        }
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
        foreach (var bot in _wrappedBots.Where(player => _debug || mainPlayerFov.Contains(player.Bot.Box)))
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
}