using darkroom.model;

namespace darkroom;

public partial class GameForm : Form
{
    private const int FormWidth = 800;
    private const int FormHeight = 800;

    private readonly Map _map;
    
    public GameForm()
    {
        _map = Map.Generate(50, 50, 3, 5, 10);
        
        InitializeComponent();
    }

    private void PaintMap(Graphics graphics)
    {
        var ratioX = FormWidth / _map.Width;
        var ratioY = FormHeight / _map.Height;

        foreach (var wall in _map.Walls)
        {
            graphics.FillRectangle(Brushes.Black, Rectangle.FromLTRB(wall.Left * ratioX,
                wall.Top * ratioY,
                wall.Right * ratioX,
                wall.Bottom * ratioY));
        }
    }
}