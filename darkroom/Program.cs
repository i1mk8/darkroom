using System.Text;
using darkroom.UI.forms.FormsData.menu;
using darkroom.UI.forms.MainForm;
using darkroom.UI.resources;

namespace darkroom;

internal static class Program
{

    [STAThread]
    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Resources.ExtractAll();
        
        ApplicationConfiguration.Initialize();
        
        MainForm.GetInstance().ShowData(Menus.GetMainMenu());
        Application.Run(MainForm.GetInstance());
    }
}