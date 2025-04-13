using darkroom.model;

namespace darkroom.UI.form;

public class PlayerWrapper(Player player, Brush color)
{
    private static readonly List<Brush> Colors = [form.Colors.PlayerFillPurple,
        form.Colors.PlayerFillYellow,
        form.Colors.PlayerFillRed,
        form.Colors.PlayerFillGreen,
        form.Colors.PlayerFillBlue];
    
    public Player Player => player;
    public Brush Color => color;

    public static List<PlayerWrapper> Wrap(List<Player> players)
    {
        var colors = new Stack<Brush>(Colors);
        var wrappedPlayers = players
            .Select(player => new PlayerWrapper(player, colors.Pop()))
            .ToList();
        return wrappedPlayers;
    }
}


