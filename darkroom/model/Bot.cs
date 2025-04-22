using darkroom.utils;

namespace darkroom.model;

public class Bot : Player
{
    private readonly float _speed;
    private readonly PathFinder _pathFinder;
    private List<Checkpoint> _path = [];

    public Bot(Map map, float width, float height, float speed) : base(map, width, height, speed)
    {
        _speed = speed;
        _pathFinder = new PathFinder(map, Box);
    }

    public void Process()
    {
        if (_path.Count == 0)
            _path = _pathFinder.FindPath(Box.DecimalCords(), GenerateRandomPoint());
        else
            ProcessPath();
    }

    private void ProcessPath()
    {
        var checkpoint = _path[0];

        if (!Utils.InaccurateEquals(checkpoint.Position.X, Box.X, _speed)
            || !Utils.InaccurateEquals(checkpoint.Position.Y, Box.Y, _speed))

        {
                
            switch (checkpoint.Direction)
            {
                case Direction.Forward:
                    MoveForward();
                    break;

                case Direction.Back:
                    MoveBack();
                    break;

                case Direction.Right:
                    MoveRight();
                    break;

                case Direction.Left:
                    MoveLeft();
                    break;
            }
        }

        else
        {
            MoveTo(checkpoint.Position.X, checkpoint.Position.Y);
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
        
        MoveTo(cords.X, cords.Y);
        var path = _pathFinder.FindPath(cords, shooterCords);
        Console.WriteLine($"Triggered On Shot: {shooterCords}");
        _path = path;
    }

    public override void Spawn()
    {
        base.Spawn();
        _path = _pathFinder.FindPath(Box.DecimalCords(), GenerateRandomPoint());
    }
}