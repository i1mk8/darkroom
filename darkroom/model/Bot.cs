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
        {
            var from = new Point((int)Math.Round(Box.X), (int)Math.Round(Box.Y));
            var to = GenerateRandomPoint();
            _path = _pathFinder.FindPath(from, to);
        }
        else
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
    }

    public override void Spawn()
    {
        base.Spawn();
        
        var to = GenerateRandomPoint();
        var from = new Point((int)Math.Round(Box.X), (int)Math.Round(Box.Y));
        _path = _pathFinder.FindPath(from, to);
    }
}