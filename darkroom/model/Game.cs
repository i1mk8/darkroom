namespace darkroom.model;

/// <summary>
/// Игровой мир, совокупность игровых моделей
/// </summary>
public class Game
{
    public readonly Map Map;
    public readonly Player Player;

    public Game()
    {
        Map = Map.Generate(50, 50, 3, 5, 10);
        Player = new Player(1, 1, Map);
        Player.SpawnPlayer();
    }
}