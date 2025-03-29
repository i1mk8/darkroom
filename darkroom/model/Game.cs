namespace darkroom.model;

/// <summary>
/// Игровой мир, совокупность игровых моделей
/// </summary>
public class Game
{
    public readonly Map Map;
    public readonly List<Player> Players = new();

    public Game()
    {
        Map = Map.Generate(50, 50, 3, 5, 10);
        var player = new Player(1, 1);
        Players.Add(player);
        SpawnPlayer(player);
    }
    
    /// <summary>
    /// Спавнит игрока в рандомной точке игровой карты
    /// </summary>
    /// <param name="player">Игрок, которого нужно заспавнить</param>
    public void SpawnPlayer(Player player)
    {
        var random = new Random();
        
        var minX = player.Box.Height;
        var maxX = Map.Width - player.Box.Width;
        
        var minY = player.Box.Height;
        var maxY = Map.Height - player.Box.Height;
        
        while (true)
        {
            var x = Math.Clamp(random.Next(0, Map.Width + 1), minX, maxX);
            var y = Math.Clamp(random.Next(0, Map.Height + 1), minY, maxY);
            
            player.MoveTo(x, y);
            
            if (!Map.Walls.Any(w => w.Contains(player.Box)))
                break;
        }
        
        Console.WriteLine($"Player: {player.Box}");
    }
}