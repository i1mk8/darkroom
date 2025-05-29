using darkroom.UI.forms.FormsData.KeyEvent;
using darkroom.UI.forms.FormsData.menu;

namespace darkroom.UI.forms.FormsData.game;

/// <summary>
/// Контроллер обработки событий ввода с клавиатуры для игры
/// </summary>
/// <param name="formData">Пользовательский интерфейс игры</param>
public class GameController(GameFormData formData) : KeyEventController
{
    protected readonly List<KeyEventAction> KeyEventsActions =
    [
        new(Keys.W, false, formData.Game.MainPlayer.MoveBack),
        new(Keys.A, false, formData.Game.MainPlayer.MoveLeft),
        new(Keys.S, false, formData.Game.MainPlayer.MoveForward),
        new(Keys.D, false, formData.Game.MainPlayer.MoveRight),

        new(Keys.Right, false, formData.Game.MainPlayer.Fov.MoveRight),
        new(Keys.Left, false, formData.Game.MainPlayer.Fov.MoveLeft),

        new(Keys.Up, true, formData.Game.MainPlayer.Shoot),
        new(Keys.Space, true, formData.Game.MainPlayer.Shoot),

        new(Keys.Oemtilde, true, () => formData.Debug = !formData.Debug),
        new(Keys.Escape, true, () =>
        {
            formData.Pause = true;
            MainForm.MainForm.GetInstance().ShowData(Menus.GetPauseMenu(formData));
        })
    ];
    
    protected override void InitializeKeyEvent(out KeyEvent.KeyEvent keyEvent) =>
        keyEvent = new KeyEvent.KeyEvent(KeyEventsActions);
}