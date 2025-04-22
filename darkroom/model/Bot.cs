using darkroom.utils;

namespace darkroom.model;

public class Bot : Player
{
    private readonly float _speed;
    
    private readonly PathFinder _pathFinder;
    private List<Point> _path = [];

    private Player? _pursuitedPlayer;

    public Bot(Map map, float width, float height, float speed) : base(map, width, height, speed)
    {
        _speed = speed;
        _pathFinder = new PathFinder(map, Box);
    }

    public void Process()
    {
        var fov = Fov.GetFov();
        
        foreach (var player in BulletProcessor.Players.Where(player => player != this && fov.Contains(player.Box)))
        {
            Console.WriteLine($"Player detected: {player.Box}");
            _path.Clear();
            _pursuitedPlayer = player;
            break;
        }

        var cords = Box.DecimalCords();
        if (_pursuitedPlayer != null)
        {
            _path = _pathFinder.FindPath(cords, _pursuitedPlayer.Box.DecimalCords());
            _pursuitedPlayer = null;
        }


        if (_path.Count > 0)
            ProcessPath();
        else if (_pursuitedPlayer == null)
            _path = _pathFinder.FindPath(cords, GenerateRandomPoint());
    }

    private void ProcessPath()
    {
        var checkpoint = _path[0];

        var xEquals = Utils.InaccurateEquals(checkpoint.X, Box.X, _speed);
        var yEquals = Utils.InaccurateEquals(checkpoint.Y, Box.Y, _speed);

        if (!xEquals || !yEquals)
        {
            var angle = -1f;
            
            if (!xEquals)
            {
                if (checkpoint.X - Box.X > 0)
                {
                    MoveRight(); 
                    angle = 0;
                }
                else
                {
                    MoveLeft();
                    angle = 180;
                }
            }
            
            if (!yEquals)
            {
                if (checkpoint.Y - Box.Y > 0)
                {
                    MoveForward();
                    angle = angle switch
                    {
                        0 => 45,
                        180 => 135,
                        _ => 90
                    };
                }
                else
                {
                    MoveBack();
                    angle = angle switch
                    {
                        0 => 315,
                        180 => 225,
                        _ => 270
                    };
                }
            }
            
            if (angle - Fov.BaseAngle > 0)
                Fov.MoveRight();
            else
                Fov.MoveLeft();
        }
        else
        {
            MoveTo(checkpoint.X, checkpoint.Y);
            _path.RemoveAt(0); 
        }
    }

    public void NotifyAboutShot(Player shooter)
    {
        if (shooter == this)
            return;
        
        var cords = Box.DecimalCords();
        var shooterCords = shooter.Box.DecimalCords();

        const float maxDistance = 20f;
        if (!(Box.DistanceTo(shooter.Box) <= maxDistance))
            return;
        
        var path = _pathFinder.FindPath(cords, shooterCords);
        _path = path;
        
        Console.WriteLine($"Triggered On Shot: {shooterCords}");
    }

    public override void Spawn()
    {
        base.Spawn();
        _path = _pathFinder.FindPath(Box.DecimalCords(), GenerateRandomPoint());
    }
}