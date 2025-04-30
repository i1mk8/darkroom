using darkroom.UI.sound;

namespace darkroom.model;

/// <summary>
/// Игровой мир, совокупность игровых моделей
/// </summary>
public class Game
{
    public readonly Map Map;
    public readonly BulletProcessor BulletProcessor;
    public readonly Player MainPlayer;
    public readonly List<Bot> Bots = [];

    public Game()
    {
        const int mapWidth = 80;
        const int mapHeight = 45;
        const int mapWallOffset = 3;
        const int minMapWallSize = 5;
        const int maxMapWallSize = 10;
        Map = Map.Generate(mapWidth, mapHeight, mapWallOffset, minMapWallSize, maxMapWallSize);
        
        BulletProcessor = new BulletProcessor(Map);

        const float playerWidth = 1f;
        const float playerHeight = 1f;
        const float playerSpeed = 0.2f;
        
        MainPlayer = new Player(Map, playerWidth, playerHeight, playerSpeed);
        var soundController = new SoundController(MainPlayer);
        MainPlayer.Initialize(BulletProcessor, soundController);

        for (var i = 0; i < 3; i++)
        {
            var bot = new Bot(Map, playerWidth, playerHeight, playerSpeed);
            bot.Initialize(BulletProcessor, soundController);
            Bots.Add(bot);
        }
    }

    public void Tick()
    {
        BulletProcessor.Process();
        Parallel.ForEach(Bots, bot => bot.Process());
    }
}