using darkroom.model;

namespace darkroom.UI.form;

public class BotWrapper(Bot bot, PlayerColor color)
{
    private static readonly List<PlayerColor> BotColors = [Colors.PlayerPurple,
        Colors.PlayerYellow,
        Colors.PlayerRed,
        Colors.PlayerGreen];
    
    public readonly Bot Bot = bot;
    public readonly PlayerColor Color = color;

    public static List<BotWrapper> Wrap(List<Bot> bots)
    {
        var colors = new Stack<PlayerColor>(BotColors);
        var wrappedBots = bots
            .Select(bot => new BotWrapper(bot, colors.Pop()))
            .ToList();
        return wrappedBots;
    }
}
