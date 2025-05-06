using darkroom.game.bot;
using darkroom.game.bullet;
using darkroom.game.player;
using darkroom.UI.sound;

namespace darkroom.game;

/// <summary>
/// Игра, совокупность игровых моделей
/// </summary>
public class Game
{
    private const int MapWidth = 80;
    private const int MapHeight = 45;
    private const int WallOffset = 3;
    private const int WallMinSize = 5;
    private const int WallMaxSize = 10;
    
    private const float PlayerWidth = 1f;
    private const float PlayerHeight = 1f;
    private const float PlayerSpeed = 0.15f;

    private const int BotsCount = 3;
    
    public readonly Map Map;
    public readonly BulletController BulletController;
    public readonly Player MainPlayer;
    public readonly List<Bot> Bots = [];

    public Game()
    {
        Map = Map.Generate(MapWidth, MapHeight, WallOffset, WallMinSize, WallMaxSize);
        
        BulletController = new BulletController(Map);
        
        MainPlayer = new Player(Map, PlayerWidth, PlayerHeight, PlayerSpeed);
        var soundController = new SoundManager(MainPlayer);
        MainPlayer.Initialize(BulletController, soundController);

        for (var i = 0; i < BotsCount; i++)
        {
            var bot = new Bot(Map, PlayerWidth, PlayerHeight, PlayerSpeed);
            bot.Initialize(BulletController, soundController);
            Bots.Add(bot);
        }
    }
    
    /// <summary>
    /// Игровой тик
    /// </summary>
    public virtual void Tick()
    {
        BulletController.Process();
        Parallel.ForEach(Bots, bot => bot.Process());
    }
}