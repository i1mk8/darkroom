using darkroom.UI.forms.FormsData;
using darkroom.UI.resources;
using darkroom.utils;
using Microsoft.Extensions.Logging;
using Timer = System.Windows.Forms.Timer;

namespace darkroom.UI.forms.MainForm;

/// <summary>
/// Главная и единственная форма. Представляет собой контейнер для запуска пользовательских сцен
/// </summary>
public sealed partial class MainForm : Form
{
    private static MainForm? _instance;
    
    private readonly ILogger _logger = Utils.GetLogger<MainForm>();
    private IFormData? _currentFormData;

    private MainForm()
    {
        KeyPreview = true;
        DoubleBuffered = true;
        Icon = new Icon(Resources.IconImagePath, 32, 32);
        WindowState = FormWindowState.Maximized;
        FormBorderStyle = FormBorderStyle.None;
        Cursor.Hide();

        InitializeTimer();
        InitializeComponent();
    }
    
    /// <summary>
    /// Инициализирует таймер формы
    /// </summary>
    private void InitializeTimer()
    {
        var timer = new Timer();
        timer.Interval = 1;

        var fpsCounter = new FpsCounter();
        timer.Tick += (_, _) =>
        {
            _currentFormData?.OnTimerTick();
            Invalidate();
            
            var currentFps = fpsCounter.Update();
            if (currentFps != null)
                _logger.LogInformation("Fps: {fps}", currentFps);
        };
        
        timer.Start();
    }
    
    /// <summary>
    /// Возвращает текущий объект формы
    /// </summary>
    /// <returns>Текущий объект формы</returns>
    public static MainForm GetInstance() => _instance ??= new MainForm();

    /// <summary>
    /// Отображает сцену пользовательского интерфейса
    /// </summary>
    /// <param name="data">Сцена пользовательского интерфейса</param>
    public void ShowData(IFormData data)
    {
        if (_currentFormData != null)
        {
            KeyDown -= _currentFormData.keyEvent.KeyDown;
            KeyUp -= _currentFormData.keyEvent.KeyUp;
        }
        
        _currentFormData = data;
        KeyDown += _currentFormData.keyEvent.KeyDown;
        KeyUp += _currentFormData.keyEvent.KeyUp;
    }
}