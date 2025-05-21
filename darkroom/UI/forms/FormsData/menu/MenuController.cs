using darkroom.UI.forms.FormsData.KeyEvent;
using darkroom.UI.resources;
using darkroom.UI.sound;

namespace darkroom.UI.forms.FormsData.menu;

/// <summary>
/// Контроллер обработки событий ввода с клавиатуры для меню
/// </summary>
/// <param name="formData">Пользовательский интерфейс меню</param>
public class MenuController(MenuFormData formData) : KeyEventController
{
    private readonly Sound _menuSound = new(Resources.MenuSoundPath);
    
    /// <summary>
    /// Вызывается при перемещении фокуса на элементах меню
    /// </summary>
    private void OnMenuMove() => _menuSound.PlaySound(1);
    
    /// <summary>
    /// Вызывается при выборе элемента меню
    /// </summary>
    private void OnMenuSelect()
    {
        _menuSound.PlaySound(1);
        formData.Menu.Select();
    }

    protected override void InitializeKeyEvent(out KeyEvent.KeyEvent keyEvent)
    {
        var keyEventsActions = new List<KeyEventAction>
        {
            new(Keys.Up, true, () =>
            {
                formData.Menu.MoveBackward();
                OnMenuMove();
            }),
            new(Keys.Down, true, () =>
            {
                formData.Menu.MoveForward();
                OnMenuMove();
            }),
            new(Keys.Space, true, OnMenuSelect),
            new(Keys.Enter, true, OnMenuSelect),
        };
        keyEvent = new KeyEvent.KeyEvent(keyEventsActions);
    }
}