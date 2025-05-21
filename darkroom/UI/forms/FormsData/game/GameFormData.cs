using darkroom.UI.forms.FormsData.menu;
using darkroom.utils;

namespace darkroom.UI.forms.FormsData.game;

/// <summary>
/// Пользоывательский интерфейс игры
/// </summary>
public class GameFormData : IFormData
{
    private readonly darkroom.game.Game _game;

    private readonly Scale _scale;
    
    private readonly List<BotWrapper> _wrappedBots;
    private readonly List<RectangleF> _scaledWalls;

    private bool _debug;
    private bool _pause;
    
    private readonly KeyEvent _keyEvent;
    public KeyEvent keyEvent => _keyEvent;

    public GameFormData()
    {
        _game = GetGame();
        
        _scale = new Scale(_game.Map.Width, _game.Map.Height);
        _scaledWalls = _game.Map.Walls.Select(wall => _scale.ScaleRectangle(wall)).ToList();
        _wrappedBots = BotWrapper.Wrap(_game.Bots);
        
        InitializeKeyEvent(out _keyEvent);
    }
    
    /// <summary>
    /// Возвращает объект игры
    /// </summary>
    /// <returns>Игра, совокупность игровых моделей</returns>
    protected virtual darkroom.game.Game GetGame() => new();
    
    /// <summary>
    /// Возвращает сцену пользовательского интерфейса, отображаемая при перезапусе текущей сцены 
    /// </summary>
    /// <returns>Сцена пользовательского интерфейса</returns>
    protected virtual IFormData GetRepeatFormData()  => new GameFormData();
    
    /// <summary>
    /// Инициализирует меню паузы
    /// </summary>
    /// <returns>Меню паузы</returns>
    private MenuFormData GetPauseMenu()
    {
        var menuItems = new List<MenuItem>
        {
            new("ПРОДОЛЖИТЬ", () =>
            {
                _pause = false;
                _keyEvent.PressedKeys.Clear();
                MainForm.MainForm.GetInstance().ShowData(this);
            }),
            new("ЗАНОВО", () => MainForm.MainForm.GetInstance().ShowData(GetRepeatFormData())),
            new("ГЛАВНОЕ МЕНЮ", () => MainForm.MainForm.GetInstance().ShowData(MenuFormData.GetMainMenu()))
        };
        return new MenuFormData("ПАУЗА", new Menu(menuItems));
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
            new(Keys.Escape, true, () =>
            {
                _pause = true;
                MainForm.MainForm.GetInstance().ShowData(GetPauseMenu());
            })
        };
        keyEvent = new KeyEvent(keyEventsActions);
    }
    
    public void OnTimerTick()
    {
        if (_pause)
            return;
        _game.Tick();
        _keyEvent.Proceed();
    }
    
    /// <summary>
    /// Отрисовывает статистику игроков
    /// </summary>
    /// <param name="graphics">Графика для рисования</param>
    protected virtual void PaintStats(Graphics graphics)
    {
        const string fontName = "Arial";
        const int fontSize = 15;
        const FontStyle fontStyle = FontStyle.Bold;
        const float horizontalSpacing = 15f;
        const int verticalPosition = 20;
        
        var players = new List<(PlayerColor Color, int Kills)>
        {
            (Colors.PlayerBlue, _game.MainPlayer.KillsCount)
        };
        players.AddRange(_wrappedBots.Select(bot => (bot.Color, bot.Bot.KillsCount)));

        var font = new Font(fontName, fontSize, fontStyle);
        var entries = players.Select(p => 
        {
            var text = $"{p.Color.ColorName}: {p.Kills}";
            var size = graphics.MeasureString(text, font);
            return (text, p.Color.Brush, size.Width, p.Kills);
        }).ToList();
        
        var totalWidth = entries.Sum(e => e.Width + horizontalSpacing);
        var x = (Screen.PrimaryScreen.Bounds.Width - totalWidth) / 2;
        var y = verticalPosition;
        
        foreach (var entry in entries.OrderByDescending(e => e.Kills))
        {
            graphics.DrawString(entry.text, font, entry.Brush, x, y);
            x += entry.Width + horizontalSpacing;
        }
    }
    
    /// <summary>
    /// Отрисовывает пули в поле зрения главного игрока
    /// </summary>
    /// <param name="graphics">Графика для рисования</param>
    /// <param name="mainPlayerFov">Поле зрения главного игрока</param>
    private void PaintBullets(Graphics graphics, Polygon mainPlayerFov)
    {
        foreach (var bullet in _game.BulletController.Bullets.Where(bullet =>
                     _debug || mainPlayerFov.Contains(bullet.Box)))
        {
            var color = Colors.PlayerBlue.Brush;
            if (bullet.Shooter != _game.MainPlayer)
                color = _wrappedBots.FirstOrDefault(bot => bot.Bot == bullet.Shooter)!.Color.Brush;
            
            graphics.FillRectangle(color, _scale.ScaleRectangle(bullet.Box));
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
        foreach (var wall in _scaledWalls)
        {
            graphics.DrawRectangle(Colors.WallPen, wall);
            graphics.FillRectangle(Colors.WallBrush, wall);
        }
    }
    
    /// <summary>
    /// Отрисовывает игроков
    /// </summary>
    /// <param name="graphics">Графика для рисования</param>
    /// <param name="mainPlayerFov">Поле зрения главного игрока</param>
    private void PaintPlayers(Graphics graphics, Polygon mainPlayerFov)
    {
        graphics.FillRectangle(Colors.PlayerBlue.Brush, _scale.ScaleRectangle(_game.MainPlayer.Box));
        foreach (var bot in _wrappedBots.Where(player => _debug || mainPlayerFov.Contains(player.Bot.Box)))
        {
            if (_debug)
            {
                var botFov = bot.Bot.Fov.GetFov();
                if (botFov.Vertices.Count >= 3)
                    PaintFov(graphics, botFov, bot.Color.Pen);
            }
            
            graphics.FillRectangle(bot.Color.Brush, _scale.ScaleRectangle(bot.Bot.Box));
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
        var vertices = _scale.ScalePolygon(fov).Vertices.ToArray();
        graphics.FillPolygon(Colors.FovBrush, vertices);
        graphics.DrawPolygon(Colors.FovPen, vertices);
        graphics.DrawLine(aimColor, vertices[0], vertices[vertices.Length / 2]);
    }

    public void OnPaint(Graphics graphics)
    {
        PaintMap(graphics);

        var fov = _game.MainPlayer.Fov.GetFov();
        if (fov.Vertices.Count >= 3)
        {
            PaintFov(graphics, fov, Colors.PlayerBlue.Pen);
            PaintPlayers(graphics, fov);
            PaintBullets(graphics, fov);
        }
        
        PaintStats(graphics);
    }
}