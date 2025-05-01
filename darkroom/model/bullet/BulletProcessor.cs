using darkroom.model.player;

namespace darkroom.model.bullet;

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
    /// Основной метод обработки полета пуль
    /// </summary>
    public void Process()
    {
        foreach (var bullet in Bullets.ToList())
        {
            MoveBullet(bullet);
            
            if (IsBulletIntersectingMap(bullet) || IsBulletIntersectingPlayers(bullet))
                Bullets.Remove(bullet);
        }
    }

    /// <summary>
    /// Перемещает пулю согласно ее направлению и скорости
    /// </summary>
    /// <param name="bullet">Пуля</param>
    private void MoveBullet(Bullet bullet)
    {
        var newX = bullet.Box.X + bullet.Direction.X * bullet.Speed;
        var newY = bullet.Box.Y + bullet.Direction.Y * bullet.Speed;
        bullet.MoveTo(newX, newY);
    }

    /// <summary>
    /// Проверяет столкновение пули с окружением (границы карты и стены)
    /// </summary>
    /// <param name="bullet">Пуля</param>
    private bool IsBulletIntersectingMap(Bullet bullet)
    {
        var wall = map.FindIntersect(bullet.Box);
        if (wall == null)
            return false;

        Console.WriteLine($"Bullet Intersects Wall: {wall}");
        return true;
    }

    /// <summary>
    /// Проверяет столкновение пули с игроками
    /// </summary>
    /// <param name="bullet">Пуля</param>
    private bool IsBulletIntersectingPlayers(Bullet bullet)
    {
        foreach (var player in Players
                     .Where(player => player != bullet.Shooter && player.Box.IntersectsWith(bullet.Box)))
        {
            HandlePlayerHit(player, bullet);
            return true;
        }

        return false;
    }
    
    /// <summary>
    /// Обрабатывает попадание пули в игрока
    /// </summary>
    /// <param name="target">Игрок=</param>
    /// <param name="bullet">Пуля</param>
    private void HandlePlayerHit(Player target, Bullet bullet)
    {
        if (!target.TakeShot())
            return;

        Console.WriteLine($"Bullet Intersects Player: {target.Box}");
        bullet.Shooter.KillsCount++;
    }
}