using darkroom.UI.forms.FormsData.KeyEvent;

namespace darkroom.UI.forms.FormsData;

/// <summary>
/// Характеризует сцену пользовательского интерфейса
/// </summary>
public interface IFormData
{
    /// <summary>
    /// Обработчик нажатия клавиш
    /// </summary>
    public KeyEventController keyEventController { get; }
    
    /// <summary>
    /// Вызывается на каждом игровом тике
    /// </summary>
    public void OnTimerTick();
    
    /// <summary>
    /// Вызывется при перерисовке пользовательского интерфейса
    /// </summary>
    /// <param name="graphics">Графика для рисования</param>
    public void OnPaint(Graphics graphics);
}