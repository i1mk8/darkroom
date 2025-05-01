namespace darkroom.UI.form;

/// <summary>
/// Обработчик нажатия клавиш
/// </summary>
/// <param name="actions">Действия, которые будут выполнены при нажатии клавиш</param>
public class KeyEvent(List<KeyEventAction> actions)
{
    private readonly List<Keys> _pressedKeys = [];
    
    /// <summary>
    /// Вызывается при нажатии клавиши
    /// </summary>
    public void KeyDown(object? sender, KeyEventArgs e)
    {
        if (_pressedKeys.Contains(e.KeyCode)) 
            return;
        
        _pressedKeys.Add(e.KeyCode);
        foreach (var action in actions.Where(action => action.KeyCode == e.KeyCode && action.Single))
            action.Action();
    }
    
    /// <summary>
    /// Вызывается при отпускании клавиши
    /// </summary>
    public void KeyUp(object? sender, KeyEventArgs e)
    {
        if (_pressedKeys.Contains(e.KeyCode))
            _pressedKeys.Remove(e.KeyCode);
    }
    
    /// <summary>
    /// Выполняет действия для клавиш, удерживаемых в нажатом состоянии
    /// </summary>
    public void Proceed()
    {
        foreach (var action in _pressedKeys.SelectMany(key=>
                     actions.Where(action => action.KeyCode == key && !action.Single)))
            action.Action();
    }
}

/// <summary>
/// Действие, выполняемое нажатием на клавишу
/// </summary>
/// <param name="keyCode">Код клавиши</param>
/// <param name="single">Флаг, указывающий, должно ли действие выполняться однократно</param>
/// <param name="action">Действие</param>
public class KeyEventAction(Keys keyCode, bool single, Action action)
{
    public readonly Keys KeyCode = keyCode;
    public readonly bool Single = single;
    public readonly Action Action = action;
}