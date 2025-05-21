using darkroom.UI.forms.FormsData.KeyEvent;
using darkroom.utils;

namespace darkroom.UI.forms.FormsData.game;

/// <summary>
/// Пользовательский интерфейс игры
/// </summary>
public class GameFormData : IFormData
{
    private const int OriginalWidth = 1920;
    private const int OriginalHeight = 1080;
    
    public readonly darkroom.game.Game Game;

    private readonly Scale _gameScale;
    private readonly Scale _interfaceScale;
    
    private readonly List<BotWrapper> _wrappedBots;
    private readonly List<RectangleF> _scaledWalls;

    private readonly Font _font;

    public bool Debug;
    public bool Pause;
    
    public KeyEventController keyEventController { get; }

    public GameFormData()
    {
        Game = GetGame();
        
        _gameScale = new Scale(Game.Map.Width, Game.Map.Height);
        _interfaceScale = new Scale(OriginalWidth, OriginalHeight);
        
        _scaledWalls = Game.Map.Walls.Select(wall => _gameScale.ScaleRectangle(wall)).ToList();
        _wrappedBots = BotWrapper.Wrap(Game.Bots);
        
        InitializeFont(out _font);
        
        keyEventController = new GameController(this);
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
    public virtual IFormData GetRepeatFormData()  => new GameFormData();
    
    public void OnTimerTick()
    {
        if (Pause)
            return;
        Game.Tick();
        keyEventController.KeyEvent.Proceed();
    }
    
    /// <summary>
    /// Инициализирует шрифт
    /// </summary>
    /// <param name="font">Шрифт</param>
    private void InitializeFont(out Font font)
    {
        const string fontName = "Arial";
        const int fontSize = 15;
        const FontStyle fontStyle = FontStyle.Bold;
        
        font = new Font(fontName, _interfaceScale.ScaleNum(fontSize), fontStyle);
    }
    
    /// <summary>
    /// Отрисовывает статистику игроков
    /// </summary>
    /// <param name="graphics">Графика для рисования</param>
    protected virtual void PaintStats(Graphics graphics)
    {
        const int horizontalSpacing = 15;
        const int verticalPosition = 20;
        
        var scaledHorizontalSpacing = _interfaceScale.ScaleNum(horizontalSpacing);
        var scaledVerticalPosition = _interfaceScale.ScaleNum(verticalPosition);
        
        var players = new List<(PlayerColor Color, int Kills)>
        {
            (Colors.PlayerBlue, Game.MainPlayer.KillsCount)
        };
        players.AddRange(_wrappedBots.Select(bot => (bot.Color, bot.Bot.KillsCount)));
        
        var entries = players.Select(p => 
        {
            var text = $"{p.Color.ColorName}: {p.Kills}";
            var size = graphics.MeasureString(text, _font);
            return (text, p.Color.Brush, size.Width, p.Kills);
        }).ToList();
        
        var totalWidth = entries.Sum(e => e.Width + scaledHorizontalSpacing);
        var x = (Screen.PrimaryScreen.Bounds.Width - totalWidth) / 2;
        
        foreach (var entry in entries.OrderByDescending(e => e.Kills))
        {
            graphics.DrawString(entry.text, _font, entry.Brush, x, scaledVerticalPosition);
            x += entry.Width + scaledHorizontalSpacing;
        }
    }
    
    /// <summary>
    /// Отрисовывает пули в поле зрения главного игрока
    /// </summary>
    /// <param name="graphics">Графика для рисования</param>
    /// <param name="mainPlayerFov">Поле зрения главного игрока</param>
    private void PaintBullets(Graphics graphics, Polygon mainPlayerFov)
    {
        foreach (var bullet in Game.BulletController.Bullets.Where(bullet =>
                     Debug || mainPlayerFov.Contains(bullet.Box)))
        {
            var color = Colors.PlayerBlue.Brush;
            if (bullet.Shooter != Game.MainPlayer)
                color = _wrappedBots.FirstOrDefault(bot => bot.Bot == bullet.Shooter)!.Color.Brush;
            
            graphics.FillRectangle(color, _gameScale.ScaleRectangle(bullet.Box));
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
        graphics.FillRectangle(Colors.PlayerBlue.Brush, _gameScale.ScaleRectangle(Game.MainPlayer.Box));
        foreach (var bot in _wrappedBots.Where(player => Debug || mainPlayerFov.Contains(player.Bot.Box)))
        {
            if (Debug)
            {
                var botFov = bot.Bot.Fov.GetFov();
                if (botFov.Vertices.Count >= 3)
                    PaintFov(graphics, botFov, bot.Color.Pen);
            }
            
            graphics.FillRectangle(bot.Color.Brush, _gameScale.ScaleRectangle(bot.Bot.Box));
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
        var vertices = _gameScale.ScalePolygon(fov).Vertices.ToArray();
        graphics.FillPolygon(Colors.FovBrush, vertices);
        graphics.DrawPolygon(Colors.FovPen, vertices);
        graphics.DrawLine(aimColor, vertices[0], vertices[vertices.Length / 2]);
    }

    public void OnPaint(Graphics graphics)
    {
        PaintMap(graphics);

        var fov = Game.MainPlayer.Fov.GetFov();
        if (fov.Vertices.Count >= 3)
        {
            PaintFov(graphics, fov, Colors.PlayerBlue.Pen);
            PaintPlayers(graphics, fov);
            PaintBullets(graphics, fov);
        }
        
        PaintStats(graphics);
    }
}