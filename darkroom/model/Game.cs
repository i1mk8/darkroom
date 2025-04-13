namespace darkroom.model;

/// <summary>
/// Игровой мир, совокупность игровых моделей
/// </summary>
public class Game
{
    public readonly Map Map;
    private readonly BulletProcessor _bulletProcessor;
    public readonly Player MainPlayer;
    public readonly List<Player> Players = [];

    public Game()
    {
        const int mapWidth = 50;
        const int mapHeight = 50;
        const int mapWallOffset = 3;
        const int minMapWallSize = 5;
        const int maxMapWallSize = 10;
        Map = Map.Generate(mapWidth, mapHeight, mapWallOffset, minMapWallSize, maxMapWallSize);
        
        _bulletProcessor = new BulletProcessor(Map);

        const float playerWidth = 1f;
        const float playerHeight = 1f;
        const float playerSpeed = 0.2f;
        
        MainPlayer = new Player(Map, playerWidth, playerHeight, playerSpeed);
        MainPlayer.Initialize(_bulletProcessor);
        
        var player = new Player(Map, playerWidth, playerHeight, playerSpeed);
        player.Initialize(_bulletProcessor);
        Players.Add(player);
    }

    public void Tick()
    {
        _bulletProcessor.Process();
    }
}