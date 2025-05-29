using darkroom.UI.forms.FormsData.game;
using darkroom.UI.forms.FormsData.KeyEvent;
using darkroom.UI.resources;

namespace darkroom.UI.forms.FormsData.training;

/// <summary>
/// Пользовательский интерфейс обучения
/// </summary>
public class TrainingFormData : GameFormData
{
    private const string SubHint = "Нажмите enter для продолжения";
    public readonly List<string> Hints =
    [
        "Используйте wasd для перемещения",
        "Используйте стрелочки влево/вправо для поворота камеры",
        "Используйте пробел или стрелочку вверх, чтобы стрелять",
        "Используйте тильду, чтобы включить режим откладки",
        "Используйте escape, чтобы открыть паузу",
        "Обучение закончено, вы можете вернуться в главное меню, используя паузу"
    ];
    public int HintIndex;
    
    private readonly Font _font;
    private readonly Font _subFont; 

    public TrainingFormData() => InitializeFont(out _font, out _subFont);
    
    /// <summary>
    /// Инициализирует шрифт
    /// </summary>
    /// <param name="font">Шрифт</param>
    /// <param name="subFont">Вспомогиательный шрифт</param>
    private void InitializeFont(out Font font, out Font subFont)
    {
        const int fontSize = 36;
        const int subFontSize = 24;
        
        FontCollection.AddFontFile(Resources.RobotoFontPath);
        font = new Font(FontCollection.Families[0], InterfaceScale.ScaleNum(fontSize));
        subFont = new Font(FontCollection.Families[0], InterfaceScale.ScaleNum(subFontSize));
    }

    private void PaintHint(Graphics graphics)
    {
        const int yOffset = 25;
        const int subYOffset = 10;

        var scaledYOffset = InterfaceScale.ScaleNum(yOffset);
        var size = graphics.MeasureString(Hints[HintIndex], _font);
        graphics.DrawString(Hints[HintIndex],
            _font,
            Colors.FontBrush, 
            (Screen.PrimaryScreen.Bounds.Width - size.Width) / 2, 
            scaledYOffset);
        
        if (HintIndex == Hints.Count - 1)
            return;

        var scaledSubYOffset = InterfaceScale.ScaleNum(subYOffset);
        var subSize = graphics.MeasureString(SubHint, _subFont);
        graphics.DrawString(SubHint,
            _subFont,
            Colors.SubFontBrush, 
            (Screen.PrimaryScreen.Bounds.Width - subSize.Width) / 2, 
            scaledYOffset + scaledSubYOffset + size.Height);
    }
    
    protected override darkroom.game.Game GetGame() => new Game();
    public override IFormData GetRepeatFormData() => new TrainingFormData();
    public override KeyEventController GetController() => new TrainingController(this);

    public override void OnPaint(Graphics graphics)
    {
        PaintMap(graphics);

        var fov = Game.MainPlayer.Fov.GetFov();
        if (fov.Vertices.Count >= 3)
        {
            PaintFov(graphics, fov, game.Colors.PlayerBlue.Pen);
            PaintPlayers(graphics, fov);
            PaintBullets(graphics, fov);
        }
        
        PaintHint(graphics);
    }
}

/// <summary>
/// Игра, совокупность игровых моделей с отключенными ботами
/// </summary>
internal class Game : darkroom.game.Game
{
    public override void Tick() => BulletController.Process();
    
}