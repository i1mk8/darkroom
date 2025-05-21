namespace darkroom.UI.forms.FormsData.KeyEvent;

/// <summary>
///  Базовый класс для создания контроллеров обработки событий ввода с клавиатуры
/// </summary>
public abstract class KeyEventController
{
    public readonly KeyEvent KeyEvent;

    protected KeyEventController() => InitializeKeyEvent(out KeyEvent);
    
    /// <summary>
    /// Инициализирует обработчик нажатия клавиш
    /// </summary>
    /// <param name="keyEvent">Обработчик нажатия клавиш</param>
    protected abstract void InitializeKeyEvent(out KeyEvent keyEvent);
}