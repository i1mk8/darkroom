namespace darkroom.model;

/// <summary>
/// Обработчик полета пуль
/// </summary>
/// <param name="map">Игровая карта</param>
public class BulletProcessor(Map map)
{
    public readonly List<Bullet> Bullets = [];
    public readonly List<Player> Players = [];
    
    /// <summary>
    /// Добавляет пулю в обработку
    /// </summary>
    /// <param name="bullet">Пуля</param>
    public void AddBullet(Bullet bullet) => Bullets.Add(bullet);
    
    /// <summary>
    /// Добавляет игрока в обработку
    /// </summary>
    /// <param name="player">Игрок</param>
    public void AddPlayer(Player player) => Players.Add(player);
    
    /// <summary>
    /// Обрабатывает полет пуль
    /// </summary>
    public void Process()
    {
        const float speedOffset = 0.05f; 
        
        foreach (var bullet in Bullets.ToList())
        {
            for (var i = 0f; i < bullet.Speed; i += speedOffset)
            {
                bullet.MoveTo(bullet.Box.X + bullet.Direction.X * speedOffset, bullet.Box.Y + bullet.Direction.Y * speedOffset);

                var intersects = false;
                if (map.FindIntersect(bullet.Box) != null)
                {
                    Console.WriteLine($"Bullet Intersects Wall: {map.FindIntersect(bullet.Box)}");
                    intersects = true;
                }
                
                foreach (var player in Players)
                {
                    if (player == bullet.OriginPlayer || !player.Box.IntersectsWith(bullet.Box))
                        continue;
                    
                    Console.WriteLine($"Bullet Intersects Player: {player.Box}");
                    player.Spawn();
                    
                    intersects = true;
                    break;
                }

                if (intersects)
                {
                    Bullets.Remove(bullet);
                    break;
                }
            }
        }
    }
}