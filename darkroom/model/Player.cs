using System.Diagnostics;
using darkroom.UI.sound;

namespace darkroom.model;

/// <summary>
/// Игрок
/// </summary>
/// <param name="map">Игровая карта</param>
/// <param name="width">Длина игрока</param>
/// <param name="height">Ширина игрока</param>
/// <param name="speed">Скорость игрока</param>
public class Player(Map map, float width, float height, float speed)
{
    protected const float BulletWidth = 1f;
    protected const float BulletHeight = 1f;
    protected const float BulletSpeed = 10f;
    
    protected const float ViewDistance = 10;
    protected const float ViewAngle = 90f;
    protected const float BaseAngleSpeed = 5f;

    public int KillsCount;
    private readonly Stopwatch _takeBulletStopwatch = new();
    
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
        
        _takeBulletStopwatch.Start();
        Spawn();
        
        const float angleOffset = 0.5f;
        const float distanceOffset = 0.05f;
        Fov = new Fov(map, this, ViewDistance, ViewAngle, BaseAngleSpeed, angleOffset, distanceOffset);
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
        Console.WriteLine($"Player: {Box}");
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
        var bullet = new Bullet(this, BulletWidth, BulletHeight, BulletSpeed);
        BulletProcessor.AddBullet(bullet);
        
        foreach (var bot in BulletProcessor.Players.OfType<Bot>())
            bot.NotifyAboutShot(bullet.Shooter);
        
        _soundController.PlayShootSound(this);
    }

    /// <summary>
    /// Вызывается при попадании пули в игрока. Если после очередного попадания прошло мало времени, то оно не учитывается 
    /// </summary>
    /// <returns>Учитывается ли попадание</returns>
    public bool TakeShot()
    {
        const long maxProtectTime = 3000;
        if (_takeBulletStopwatch.ElapsedMilliseconds < maxProtectTime)
            return false;
        
        Spawn();
        _takeBulletStopwatch.Restart();
        return true;
    }
}