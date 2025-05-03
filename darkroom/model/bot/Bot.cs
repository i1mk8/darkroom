using darkroom.model.bullet;
using darkroom.model.player;
using darkroom.UI.sound;
using darkroom.utils;
using Microsoft.Extensions.Logging;

namespace darkroom.model.bot;

/// <summary>
/// Бот (игрок, управлемый ИИ)
/// </summary>
public class Bot : Player
{
    private const float FovAngleOffset = 2f;
    private const float FovDistanceOffset = 0.5f;
    
    private const float MaxReactionDistance = 20f;
    private const float CollisionCheckOffset = 0.05f;
    
    private readonly ILogger _logger = Utils.GetLogger<Bot>();
    
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
        InitializeFov();
    }
    
    /// <summary>
    /// Инициализация поля зрения бота
    /// </summary>
    private void InitializeFov() =>
        Fov = new Fov(_map, this, ViewDistance, ViewAngle, BaseAngleSpeed, FovAngleOffset, FovDistanceOffset);

    /// <summary>
    /// Обрабатывает логику поведения бота
    /// </summary>
    public void Process()
    {   
        var visiblePlayers = GetPlayersInFov();
        
        foreach (var player in visiblePlayers)
        {
            HandlePlayerDetection(player);
            if (TryShootPlayer(player))
                return;
        }
        
        ProcessMovement();
    }
    
    /// <summary>
    /// Получает игроков в поле зрения бота
    /// </summary>
    /// <returns>Игроки в поле зрения бота</returns>
    private IEnumerable<Player> GetPlayersInFov()
    {
        var fov = Fov.GetFov();
        return BulletProcessor.Players
            .Where(player => player != this && fov.Contains(player.Box));
    }
    
    /// <summary>
    /// Обработка попадания игрока в поле зрения бота
    /// </summary>
    /// <param name="player">Игрок, попавший в поле зрения</param>
    private void HandlePlayerDetection(Player player)
    {
        _logger.LogDebug("Обнаружен игрок: {box}", player.Box);
        
        var direction = CalculateDirectionTo(player);
        RotateFovTowards(direction);
        
        if (_shouldUpdatePath)
            _path = _pathFinder.FindPath(Box.DecimalCords(), player.Box.DecimalCords());
    }
    
    /// <summary>
    /// Пытается выстрелить по игроку
    /// </summary>
    /// <param name="player">Игрок, по которому производится выстрел</param>
    /// <returns>true - успешный выстрел; false - выстрел не возможен</returns>
    private bool TryShootPlayer(Player player)
    {
        if (!CanShoot(player)) 
            return false;
        
        Shoot();
        return true;
    }
    
    /// <summary>
    /// Обработка движения бота
    /// </summary>
    private void ProcessMovement()
    {
        if (_path.Count > 0)
            FollowPath();
        else
            GenerateNewRandomPath();
    }
    
    /// <summary>
    /// Обработка движения по пути
    /// </summary>
    private void FollowPath()
    {
        var nextPoint = _path[0];
        var reached = IsPointReached(nextPoint);

        if (!reached)
        {
            MoveTowards(nextPoint);
            RotateFovTowardsTarget(nextPoint);
        }
        else
        {
            MoveTo(nextPoint.X, nextPoint.Y);
            _path.RemoveAt(0);
        }
    }
    
    /// <summary>
    /// Проверка достижения точки пути
    /// </summary>
    /// <param name="point">Точка пути</param>
    private bool IsPointReached(Point point) => 
        Utils.InaccurateEquals(point.X, Box.X, _speed) && 
        Utils.InaccurateEquals(point.Y, Box.Y, _speed);
    
    /// <summary>
    /// Генерация нового случайного пути
    /// </summary>
    private void GenerateNewRandomPath()
    {
        _shouldUpdatePath = true;
        _path = _pathFinder.FindPath(Box.DecimalCords(), GenerateRandomPoint());
    }
    
    /// <summary>
    /// Метод, проверяющий может ли пуля, выпущенная ботом, долеть до игрока или бота
    /// </summary>
    /// <param name="player">Игрок или бот, в которого летит пуля</param>
    private bool CanShoot(Player player)
    {
        var origin = Box.Center();
        var target = player.Box.Center();
        var direction = CalculateBulletDirection(origin, target);
        
        return !IsBulletIntersectingMap(origin, direction, origin.DistanceTo(target));
    }
    
    /// <summary>
    /// Расчет направления полета пули
    /// </summary>
    /// <param name="origin">Стартовая точка полета пули</param>
    /// <param name="target">Конечная точка полета пули</param>
    /// <returns>Направление полета пули</returns>
    private PointF CalculateBulletDirection(PointF origin, PointF target)
    {
        var direction = new PointF(target.X - origin.X, target.Y - origin.Y);
        var distance = origin.DistanceTo(target);
        return direction.NormalizeDirection(distance);
    }
    
    /// <summary>
    /// Проверка столкновений пули с окружением (границы карты и стены)
    /// </summary>
    /// <param name="origin">Стартовая точка полета пули</param>
    /// <param name="direction">Направление полета пули</param>
    /// <param name="maxDistance">Максимальная дистанция полета пули</param>
    private bool IsBulletIntersectingMap(PointF origin, PointF direction, float maxDistance)
    {
        for (var i = 0f; i< maxDistance; i += CollisionCheckOffset)
        {
            var bulletBox = CreateBulletBox(origin, direction, i);
            if (_map.FindIntersect(bulletBox) != null)
            {
                _shouldUpdatePath = false;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Создает хитбокс пули
    /// </summary>
    /// <param name="origin">Стартовая точка полета пули</param>
    /// <param name="direction">Направление полета пули</param>
    /// <param name="offset">Коефициент смещения от стартовой точки</param>
    /// <returns>Хитбокс пули</returns>
    private RectangleF CreateBulletBox(PointF origin, PointF direction, float offset)
    {
        return new RectangleF(
            origin.X + direction.X * offset - BulletWidth / 2,
            origin.Y + direction.Y * offset - BulletHeight / 2,
            BulletWidth,
            BulletHeight
        );
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
    /// Поворот Fov в сторону цели
    /// </summary>
    /// <param name="target">Цель</param>
    private void RotateFovTowardsTarget(Point target)
    {
        var direction = new PointF(target.X - Box.X, target.Y - Box.Y);
        RotateFovTowards(direction);
    }
    
    /// <summary>
    /// Уведомляет бота о стрельбе другим игроком или ботом
    /// </summary>
    /// <param name="shooter">Стрелявший игрок или бот</param>
    public void NotifyAboutShot(Player shooter)
    {
        if (shooter == this)
            return;
        
        if (!ShouldReactToShot(shooter.Box))
            return;
        
        _path = _pathFinder.FindPath(Box.DecimalCords(), shooter.Box.DecimalCords());
        _logger.LogDebug("Реакция на выстрел: {box}", shooter.Box);
    }
    
    /// <summary>
    /// Определяет, должен ли бот реагировать на выстрел
    /// </summary>
    /// <param name="shooterBox">Бокс стрелющего игрока или бота</param>
    private bool ShouldReactToShot(RectangleF shooterBox)
    {
        if (Box.DistanceTo(shooterBox) > MaxReactionDistance)
            return false;
        
        return !IsLastPathPointEqualTo(shooterBox.DecimalCords());
    }

    /// <summary>
    /// Проверка совпадения последней точки текущего пути с заданной точкой
    /// </summary>
    /// <param name="point">Точка</param>
    private bool IsLastPathPointEqualTo(Point point)
    {
        if (_path.Count == 0)
            return false;
        return  Utils.InaccurateEquals(point.X, _path[^1].X, _speed) && 
                Utils.InaccurateEquals(point.Y, _path[^1].Y, _speed);
    } 

    public override void Spawn()
    {
        base.Spawn();
        _path = _pathFinder.FindPath(Box.DecimalCords(), GenerateRandomPoint());
    }
}