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
        foreach (var bullet in Bullets.ToList())
        {
            bullet.MoveTo(bullet.Box.X + bullet.Direction.X * bullet.Speed,
                bullet.Box.Y + bullet.Direction.Y * bullet.Speed);

            var intersects = false;
            if (map.FindIntersect(bullet.Box) != null)
            {
                Console.WriteLine($"Bullet Intersects Wall: {map.FindIntersect(bullet.Box)}");
                intersects = true;
            }
            
            foreach (var player in Players)
            {
                if (player == bullet.Shooter || !player.Box.IntersectsWith(bullet.Box))
                    continue;
                
                if (player.TakeShot())
                {
                    Console.WriteLine($"Bullet Intersects Player: {player.Box}");
                    bullet.Shooter.KillsCount++;
                }
                
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