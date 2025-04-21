using darkroom.model;

namespace darkroom.UI.form;

public class BotWrapper(Bot bot, Brush color)
{
    private static readonly List<Brush> Colors = [form.Colors.PlayerFillPurple,
        form.Colors.PlayerFillYellow,
        form.Colors.PlayerFillRed,
        form.Colors.PlayerFillGreen];
    
    public readonly Bot Bot = bot;
    public readonly Brush Color = color;

    public static List<BotWrapper> Wrap(List<Bot> bots)
    {
        var colors = new Stack<Brush>(Colors);
        var wrappedBots = bots
            .Select(bot => new BotWrapper(bot, colors.Pop()))
            .ToList();
        return wrappedBots;
    }
}


