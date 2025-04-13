using darkroom.model;

namespace darkroom.UI.KeyEvent;

public class SingleKeyEvent(Player player, Action onTilda)
{
    private readonly List<Keys> _pressedKeys = [];
    
    public void KeyDown(object? sender, KeyEventArgs e)
    {
        if (!_pressedKeys.Contains(e.KeyCode))
        {
            Proceed(e.KeyCode);
            _pressedKeys.Add(e.KeyCode);
        }
    }
    
    public void KeyUp(object? sender, KeyEventArgs e)
    {
        if (_pressedKeys.Contains(e.KeyCode))
            _pressedKeys.Remove(e.KeyCode);
    }

    private void Proceed(Keys key)
    {
        switch (key)
        {
            case Keys.Up:
                player.Shoot();
                break;
            
            case Keys.Oemtilde:
                onTilda();
                break;
        }
    }
}