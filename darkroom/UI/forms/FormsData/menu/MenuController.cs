using darkroom.UI.resources;
using darkroom.UI.sound;

namespace darkroom.UI.forms.FormsData.menu;

public class MenuController(MenuFormData formData)
{
    private readonly Sound _menuSound = new(Resources.MenuSoundPath);
    
    private void OnMenuMove() => _menuSound.PlaySound(1);

    private void OnMenuSelect()
    {
        _menuSound.PlaySound(1);
        formData.Menu.Select();
    }
    
    public KeyEvent GetKeyEvent()
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
        return new KeyEvent(keyEventsActions);
    }

}