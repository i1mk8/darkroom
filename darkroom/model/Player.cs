using darkroom.UI.sound;
using darkroom.utils;

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
    public RectangleF Box { get; private set; } = new(-1, -1, width, height);
    public Fov Fov { get; private set; }
    private BulletProcessor _bulletProcessor;
    private SoundController _soundController;

    /// <summary>
    /// Инициализирует поле зрения, обработчик полета пуль и спавнит игрока
    /// <param name="bulletProcessor">Обработчик полета пуль</param>
    /// </summary>
    public void Initialize(BulletProcessor bulletProcessor, SoundController soundController)
    {
        const float viewDistance = 10;
        const float viewAngle = 60f;
        const float baseAngleSpeed = 5f;
        
        _bulletProcessor = bulletProcessor;
        _bulletProcessor.AddPlayer(this);
        
        _soundController = soundController;
        
        Spawn();
        Fov = new Fov(map, this, viewDistance, viewAngle, baseAngleSpeed);
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
        const float bulletWidth = 0.5f;
        const float bulletHeight = 0.5f;
        const float bulletSpeed = 10f;

        var bullet = new Bullet(this,
            bulletWidth,
            bulletHeight,
            bulletSpeed);
        _bulletProcessor.AddBullet(bullet);
        
        foreach (var bot in _bulletProcessor.Players.OfType<Bot>())
            bot.NotifyAboutShot(bullet.Shooter);
        
        _soundController.PlayShootSound(this);
    }
}