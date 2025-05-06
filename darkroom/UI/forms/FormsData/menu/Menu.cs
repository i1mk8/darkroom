namespace darkroom.UI.forms.FormsData.menu;

/// <summary>
/// Игровое меню
/// </summary>
/// <param name="items">Элементы меню</param>
public class Menu(List<MenuItem> items)
{
    public List<MenuItem> Items
    {
        get
        {
            items[_selectedItemIndex].Selected = true;
            return items;
        }
    }
    private int _selectedItemIndex;
    
    /// <summary>
    /// Перемещает фокус вперед (на следующий эелемент меню)
    /// </summary>
    public void MoveForward()
    {
        items[_selectedItemIndex].Selected = false;
        _selectedItemIndex++;
        if (_selectedItemIndex >= items.Count)
            _selectedItemIndex = 0;
    }
    
    /// <summary>
    /// Перемаещает фокус назад (на предыдущий эелемент меню)
    /// </summary>
    public void MoveBackward()
    {
        items[_selectedItemIndex].Selected = false;
        _selectedItemIndex--;
        if (_selectedItemIndex < 0)
            _selectedItemIndex = items.Count - 1;
    }
    
    /// <summary>
    /// Выбирает сфокусированный эелемент игрового меню
    /// </summary>
    public void Select() => items[_selectedItemIndex].Action();
}

/// <summary>
/// Эелемент игрового меню
/// </summary>
/// <param name="text">Назавание эелемента</param>
/// <param name="action">Действие, вызывающиеся при выборе эелемнта игрового меню</param>
public class MenuItem(string text, Action action)
{
    public readonly string Text = text;
    public readonly Action Action = action;
    public bool Selected;
}