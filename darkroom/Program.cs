using System.Text;
using darkroom.UI.forms.FormsData.menu;
using darkroom.UI.forms.MainForm;
using darkroom.UI.resources;

namespace darkroom;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Resources.ExtractAll();
        
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        
        MainForm.GetInstance().ShowData(MenuFormData.GetMainMenu());
        Application.Run(MainForm.GetInstance());
    }
}