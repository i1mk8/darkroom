using darkroom.model;

namespace darkroom.UI;

public class KeyEvent(Player player)
{
    private bool _wPressed;
    private bool _aPressed;
    private bool _sPressed;
    private bool _dPressed;
    
    public void KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.W: _wPressed = true; break;
            case Keys.A: _aPressed = true; break;
            case Keys.S: _sPressed = true; break;
            case Keys.D: _dPressed = true; break;
        }
    }

    public void KeyUp(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.W: _wPressed = false; break;
            case Keys.A: _aPressed = false; break;
            case Keys.S: _sPressed = false; break;
            case Keys.D: _dPressed = false; break;
        }
    }
    
    public void ProcessMovement()
    {
        if (_wPressed) player.MoveBack();
        if (_aPressed) player.MoveLeft();
        if (_sPressed) player.MoveForward();
        if (_dPressed) player.MoveRight();
    }
}