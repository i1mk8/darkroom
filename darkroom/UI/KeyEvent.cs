using darkroom.model;

namespace darkroom.UI;

public class KeyEvent(Player player)
{
    private readonly List<Keys> _pressedKeys = new();
    
    public void KeyDown(object? sender, KeyEventArgs e)
    {
        if (!_pressedKeys.Contains(e.KeyCode))
            _pressedKeys.Add(e.KeyCode);
    }

    public void KeyUp(object? sender, KeyEventArgs e)
    {
        if (_pressedKeys.Contains(e.KeyCode))
            _pressedKeys.Remove(e.KeyCode);
    }
    
    public void ProceedMovement()
    {
        foreach (var key in _pressedKeys)
        {
            switch (key)
            {
                case Keys.W:
                    player.MoveBack();
                    break;
                case Keys.A:
                    player.MoveLeft();
                    break;
                case Keys.S:
                    player.MoveForward();
                    break;
                case Keys.D:
                    player.MoveRight();
                    break;
                
                case Keys.Right:
                    player.Fov.MoveRight();
                    break;
                case Keys.Left:
                    player.Fov.MoveLeft();
                    break;
            }
        }
    }
}