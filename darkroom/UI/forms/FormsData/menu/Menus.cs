using darkroom.UI.forms.FormsData.game;
using darkroom.UI.forms.FormsData.training;

namespace darkroom.UI.forms.FormsData.menu;

/// <summary>
/// Меню
/// </summary>
public static class Menus
{
    
    /// <summary>
    /// Возвращает главное меню
    /// </summary>
    /// <returns>Главное меню</returns>
    public static MenuFormData GetMainMenu()
    {
        var menuItems = new List<MenuItem>
        {
            new("ОБУЧЕНИЕ", () => MainForm.MainForm.GetInstance().ShowData(new TrainingFormData())),
            new("ИГРАТЬ", () => MainForm.MainForm.GetInstance().ShowData(new GameFormData())),
            new("ВЫХОД", Application.Exit)
        };
        return new MenuFormData("DARKROOM", new Menu(menuItems));
    }
    
    /// <summary>
    /// Возвращает меню паузы
    /// </summary>
    /// <param name="formData">Пользовательский интерфейс игры</param>
    /// <returns>Меню паузы</returns>
    public static MenuFormData GetPauseMenu(GameFormData formData)
    {
        var menuItems = new List<MenuItem>
        {
            new("ПРОДОЛЖИТЬ", () =>
            {
                formData.Pause = false;
                formData.keyEventController.KeyEvent.PressedKeys.Clear();
                MainForm.MainForm.GetInstance().ShowData(formData);
            }),
            new("ЗАНОВО", () => MainForm.MainForm.GetInstance().ShowData(formData.GetRepeatFormData())),
            new("ГЛАВНОЕ МЕНЮ", () => MainForm.MainForm.GetInstance().ShowData(GetMainMenu()))
        };
        return new MenuFormData("ПАУЗА", new Menu(menuItems));
    }
}