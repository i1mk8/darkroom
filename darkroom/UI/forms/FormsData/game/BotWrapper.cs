using darkroom.game.bot;

namespace darkroom.UI.forms.FormsData.game;

/// <summary>
/// Обёртка для бота, связывающая его с цветом
/// </summary>
public class BotWrapper(Bot bot, PlayerColor color)
{
    private static readonly List<PlayerColor> BotColors = [Colors.PlayerPurple,
        Colors.PlayerYellow,
        Colors.PlayerRed,
        Colors.PlayerGreen];
    
    public readonly Bot Bot = bot;
    public readonly PlayerColor Color = color;

    /// <summary>
    /// Создаёт список обёрток для ботов, назначая каждому уникальный цвет из доступных
    /// </summary>
    /// <param name="bots">Список ботов</param>
    /// <returns>Список обёрнутых ботов с назначенными цветами</returns>
    public static List<BotWrapper> Wrap(List<Bot> bots)
    {
        var colors = new Stack<PlayerColor>(BotColors);
        var wrappedBots = bots
            .Select(bot => new BotWrapper(bot, colors.Pop()))
            .ToList();
        return wrappedBots;
    }
}
