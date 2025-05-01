namespace darkroom.UI.form;

public class KeyEvent(List<KeyEventAction> actions)
{
    private readonly List<Keys> _pressedKeys = [];
    
    public void KeyDown(object? sender, KeyEventArgs e)
    {
        if (_pressedKeys.Contains(e.KeyCode)) 
            return;
        
        _pressedKeys.Add(e.KeyCode);
        foreach (var action in actions.Where(action => action.KeyCode == e.KeyCode && action.Single))
            action.Action();
    }

    public void KeyUp(object? sender, KeyEventArgs e)
    {
        if (_pressedKeys.Contains(e.KeyCode))
            _pressedKeys.Remove(e.KeyCode);
    }
    
    public void Proceed()
    {
        foreach (var action in _pressedKeys.SelectMany(key=>
                     actions.Where(action => action.KeyCode == key && !action.Single)))
            action.Action();
    }
}

public class KeyEventAction(Keys keyCode, bool single, Action action)
{
    public readonly Keys KeyCode = keyCode;
    public readonly bool Single = single;
    public readonly Action Action = action;
}