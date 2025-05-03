using System.Diagnostics;
using darkroom.model.bot;
using darkroom.model.bullet;
using darkroom.UI.sound;
using darkroom.utils;
using Microsoft.Extensions.Logging;

namespace darkroom.model.player;

/// <summary>
/// Игрок
/// </summary>
/// <param name="map">Игровая карта</param>
/// <param name="width">Длина игрока</param>
/// <param name="height">Ширина игрока</param>
/// <param name="speed">Скорость игрока</param>
public class Player(Map map, float width, float height, float speed)
{
    protected const float BulletWidth = 0.3f;
    protected const float BulletHeight = 0.3f;
    private const float BulletSpeed = 0.375f;
    
    private const float AngleOffset = 0.5f;
    private const float DistanceOffset = 0.05f;
    protected const float ViewDistance = 12;
    protected const float ViewAngle = 72f;
    protected const float BaseAngleSpeed = 3.75f;

    private const long ShootCooldown = 500;
    private const long TakeShotCooldown = 1000;
    
    private readonly ILogger _logger = Utils.LoggerFactory.CreateLogger<Player>();

    private readonly Stopwatch _shootStopwatch = new();
    private readonly Stopwatch _takeBulletStopwatch = new();

    public int KillsCount;
    public RectangleF Box { get; private set; } = new(-1, -1, width, height);
    public Fov Fov { get; protected set; }
    
    protected BulletProcessor BulletProcessor;
    private SoundController _soundController;

    /// <summary>
    /// Инициализирует поле зрения, обработчик полета пуль и спавнит игрока
    /// <param name="bulletProcessor">Обработчик полета пуль</param>
    /// </summary>
    public virtual void Initialize(BulletProcessor bulletProcessor, SoundController soundController)
    {
        BulletProcessor = bulletProcessor;
        BulletProcessor.AddPlayer(this);
        
        _soundController = soundController;
        
        _shootStopwatch.Start();
        _takeBulletStopwatch.Start();
        
        Spawn();
        
        Fov = new Fov(map, this, ViewDistance, ViewAngle, BaseAngleSpeed, AngleOffset, DistanceOffset);
    }

    /// <summary>
    /// Перемещает игрока в заданные координаты, если это возможно
    /// </summary>
    /// <param name="x">Координата по X</param>
    /// <param name="y">Координата по Y</param>
    /// <returns>Объект, помешавший перемещению (если он была)</returns>
    public RectangleF? MoveTo(float x, float y)
    {
        var box = new RectangleF(x, y, width, height);
        var intersect = map.FindIntersect(box);

        if (intersect != null)
            return intersect;

        Box = box;
        return null;
    }

    /// <summary>
    /// Перемещение игрока вперед
    /// </summary>
    public void MoveForward()
    {
        var intersect = MoveTo(Box.X, Box.Y + speed);
        if (intersect == null)
        {
            _soundController.PlayWalkSound(this);
            return;
        }
        MoveTo(Box.X, Box.Y + (intersect.Value.Top - Box.Bottom));

    }
    
    /// <summary>
    /// Перемещение игрока назад
    /// </summary>
    public void MoveBack()
    {
        var intersect = MoveTo(Box.X, Box.Y - speed);
        if (intersect == null)
        {
            _soundController.PlayWalkSound(this);
            return;
        }
        MoveTo(Box.X, Box.Y - (Box.Top - intersect.Value.Bottom));
    }

    /// <summary>
    /// Перемещение игрока вправо
    /// </summary>
    public void MoveRight()
    {
        var intersect = MoveTo(Box.X + speed, Box.Y);
        if (intersect == null)
        {
            _soundController.PlayWalkSound(this);
            return;
        }
        MoveTo(Box.X + (intersect.Value.Left - Box.Right), Box.Y);
    }

    /// <summary>
    /// Перемещение игрока влево
    /// </summary>
    public void MoveLeft()
    {
        var intersect = MoveTo(Box.X - speed, Box.Y);
        if (intersect == null)
        {
            _soundController.PlayWalkSound(this);
            return;
        }
        MoveTo(Box.X - (Box.Left - intersect.Value.Right), Box.Y);
    }
    
    /// <summary>
    /// Спавнит игрока в рандомной точке игровой карты
    /// </summary>
    public virtual void Spawn()
    {
        var position = GenerateRandomPoint();
        MoveTo(position.X, position.Y);
        _logger.LogDebug("Игрок: {box}", Box);
    }

    protected Point GenerateRandomPoint()
    {
        var random = new Random();
        
        var maxX = map.Width - Box.Width;
        var maxY = map.Height - Box.Height;

        while (true)
        {
            var x = (int)Math.Min(random.Next(0, map.Width + 1), maxX);
            var y = (int)Math.Min(random.Next(0, map.Height + 1), maxY);

            if (map.FindIntersect(new RectangleF(x, y, width, height)) == null)
                return new Point(x, y);
        }
    }
    
    /// <summary>
    /// Стреляет от лица игрока. Тракетория полета пули - BaseAngle из Fov (угол, характеризующий направление взгляда)
    /// </summary>
    public void Shoot()
    {
        if (_shootStopwatch.ElapsedMilliseconds < ShootCooldown)
            return;
        
        var bullet = new Bullet(this, BulletWidth, BulletHeight, BulletSpeed);
        BulletProcessor.AddBullet(bullet);
        
        foreach (var bot in BulletProcessor.Players.OfType<Bot>())
            bot.NotifyAboutShot(bullet.Shooter);
        
        _soundController.PlayShootSound(this);
        _shootStopwatch.Restart();
    }

    /// <summary>
    /// Вызывается при попадании пули в игрока. Если после очередного попадания прошло мало времени, то оно не учитывается 
    /// </summary>
    /// <param name="shooter">Игрок, сделавший выстрел</param>
    /// <returns>Учитывается ли попадание</returns>
    public bool TakeShot(Player shooter)
    {
        if (_takeBulletStopwatch.ElapsedMilliseconds < TakeShotCooldown)
            return false;

        _soundController.PlayHitSound(shooter);
        _soundController.PlayTakeShotSound(this);
        
        Spawn();
        _takeBulletStopwatch.Restart();
        return true;
    }
}