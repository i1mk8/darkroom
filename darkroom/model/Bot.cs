using darkroom.UI.sound;
using darkroom.utils;

namespace darkroom.model;

/// <summary>
/// Бот (игрок, управлемый ИИ)
/// </summary>
public class Bot : Player
{
    private readonly float _speed;
    private readonly Map _map;
    private readonly PathFinder _pathFinder;
    
    private List<Point> _path = [];
    private bool _shouldUpdatePath = true;
    
    /// <param name="map">Игровая карта</param>
    /// <param name="width">Длина игрока</param>
    /// <param name="height">Ширина игрока</param>
    /// <param name="speed">Скорость игрока</param>
    public Bot(Map map, float width, float height, float speed) : base(map, width, height, speed)
    {
        _map = map;
        _speed = speed;
        _pathFinder = new PathFinder(map, Box);
    }

    public override void Initialize(BulletProcessor bulletProcessor, SoundController soundController)
    {
        base.Initialize(bulletProcessor, soundController);
        
        const float angleOffset = 1f;
        const float distanceOffset = 0.1f;
        Fov = new Fov(_map, this, ViewDistance, ViewAngle, BaseAngleSpeed, angleOffset, distanceOffset);
    }

    /// <summary>
    /// Обрабатывает логику поведения бота
    /// </summary>
    public void Process()
    {   
        var fov = Fov.GetFov();
        
        foreach (var player in BulletProcessor.Players
                     .Where(player => player != this && fov.Contains(player.Box)))
        {
            HandlePlayerDetection(player);
            
            if (CanShoot(player))
            {
                Shoot();
                return;
            }
        }
        
        if (_path.Count > 0)
            ProcessPath();
        else
        {
            _shouldUpdatePath = true;
            _path = _pathFinder.FindPath(Box.DecimalCords(), GenerateRandomPoint());
        }
    }
    
    /// <summary>
    /// Обработка попадания игрока или бота в поле зрения бота
    /// </summary>
    /// <param name="player">Игрок или бот, попавший в поле зрения</param>
    private void HandlePlayerDetection(Player player)
    {
        Console.WriteLine($"Player Detected: {player.Box}");
        
        var direction = CalculateDirectionTo(player);
        RotateFovTowards(direction);
        
        if (_shouldUpdatePath)
            _path = _pathFinder.FindPath(Box.DecimalCords(), player.Box.DecimalCords());
    }
    
    /// <summary>
    /// Метод, проверяющий может ли пуля, выпущенная ботом, долеть до игрока или бота
    /// </summary>
    /// <param name="player">Игрок или бот, в которого летит пуля</param>
    private bool CanShoot(Player player)
    {
        var origin = Box.Center();
        var target = player.Box.Center();
        
        var direction = new PointF(target.X - origin.X, target.Y - origin.Y);
        var distance = MathF.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
        
        const float offset = 0.05f;
        direction = new PointF(direction.X / distance, direction.Y / distance);
        
        for (float t = 0; t < distance; t += offset)
        {
            var box = new RectangleF(origin.X + direction.X * t - BulletWidth / 2, 
                origin.Y + direction.Y * t - BulletHeight / 2,
                BulletWidth,
                BulletHeight);
                
            if (_map.FindIntersect(box) != null)
            {
                _shouldUpdatePath = false;
                return false;
            }
        }

        return true;
    }
    
    /// <summary>
    /// Расчитывает направление до игрока или бота
    /// </summary>
    /// <param name="player">Игрок или бот</param>
    /// <returns>Направдение до игрока или бота</returns>
    private PointF CalculateDirectionTo(Player player)
    {
        var target = player.Box.Center();
        var current = Box.Center();
        return new PointF(target.X - current.X, target.Y - current.Y);
    }
    
    /// <summary>
    /// Поворачивает направление взгяда бота до заданного направления
    /// </summary>
    /// <param name="direction">Направление</param>
    private void RotateFovTowards(PointF direction)
    {
        var targetAngle = Utils.ToDegrees(MathF.Atan2(direction.Y, direction.X));
        targetAngle = (targetAngle + 360) % 360;
        
        var angleDiff = (targetAngle - Fov.BaseAngle + 540) % 360 - 180;
        
        if (Math.Abs(angleDiff) > 1)
            if (angleDiff > 0)
                Fov.MoveRight();
            else
                Fov.MoveLeft();
    }
    
    /// <summary>
    /// Двигает бота его направление взгляда по пути
    /// </summary>
    private void ProcessPath()
    {
        var nextPoint = _path[0];
        var reachedX = Utils.InaccurateEquals(nextPoint.X, Box.X, _speed);
        var reachedY = Utils.InaccurateEquals(nextPoint.Y, Box.Y, _speed);

        if (!reachedX || !reachedY)
        {
            MoveTowards(nextPoint);
            var direction = new PointF(nextPoint.X - Box.X, nextPoint.Y - Box.Y);
            RotateFovTowards(direction);
        }
        else
        {
            MoveTo(nextPoint.X, nextPoint.Y);
            _path.RemoveAt(0);
        }
    }
    
    /// <summary>
    /// Двигает бота к заданной точке
    /// </summary>
    /// <param name="target">Точка</param>
    private void MoveTowards(Point target)
    {
        if (!Utils.InaccurateEquals(target.X, Box.X, _speed))
        {
            if (target.X > Box.X)
                MoveRight();
            else
                MoveLeft();
        }

        if (!Utils.InaccurateEquals(target.Y, Box.Y, _speed))
        {
            if (target.Y > Box.Y)
                MoveForward();
            else
                MoveBack();
        }
    }
    
    /// <summary>
    /// Уведомляет бота о стрельбе другим игроком или ботом
    /// </summary>
    /// <param name="shooter">Стрелявший игрок или бот</param>
    public void NotifyAboutShot(Player shooter)
    {
        if (shooter == this)
            return;
        
        var shooterBox = shooter.Box;
        if (!ShouldReactToShot(shooterBox))
            return;
        
        _path = _pathFinder.FindPath(Box.DecimalCords(), shooterBox.DecimalCords());
        Console.WriteLine($"Triggered On Shot: {shooterBox.DecimalCords()}");
    }
    
    /// <summary>
    /// Определяет, должен ли бот реагировать на выстрел
    /// </summary>
    /// <param name="shooterBox">Бокс стрелющего игрока или бота</param>
    private bool ShouldReactToShot(RectangleF shooterBox)
    {
        const float maxDistance = 20f;
        if (Box.DistanceTo(shooterBox) > maxDistance)
            return false;
        
        if (_path.Count == 0) return true;
        
        var lastPathPoint = _path[^1];
        var shooterPoint = shooterBox.DecimalCords();
        return !(Utils.InaccurateEquals(shooterPoint.X, lastPathPoint.X, _speed) && 
                Utils.InaccurateEquals(shooterPoint.Y, lastPathPoint.Y, _speed));
    }

    public override void Spawn()
    {
        base.Spawn();
        _path = _pathFinder.FindPath(Box.DecimalCords(), GenerateRandomPoint());
    }
}