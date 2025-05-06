namespace darkroom.UI.forms.FormsData.game;

/// <summary>
/// Пользовательский интерфейс обучения
/// </summary>
public class TrainingFormData : GameFormData
{
    protected override darkroom.game.Game GetGame() => new Game();
    protected override IFormData GetRepeatFormData() => new TrainingFormData();

    protected override void PaintStats(Graphics graphics) { }
}

/// <summary>
/// Игра, совокупность игровых моделей с отключенными ботами
/// </summary>
internal class Game : darkroom.game.Game
{
    public override void Tick() => BulletController.Process();
    
}