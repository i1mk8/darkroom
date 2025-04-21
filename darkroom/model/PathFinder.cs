namespace darkroom.model;

public class PathFinder(Map map, RectangleF box)
{
    public List<Checkpoint> FindPath(Point start, Point end)
    {
        var nodes = new Dictionary<Point, Node>();
        var priorityQueue = new PriorityQueue<Node, int>();
        var startNode = new Node(0, null, new Checkpoint(start, Direction.None));
        
        nodes.Add(start, startNode);
        priorityQueue.Enqueue(startNode, 0);

        while (priorityQueue.Count > 0)
        {
            var current = priorityQueue.Dequeue();
            if (current.Checkpoint.Position == end)
                return ReconstructPath(current);

            foreach (var neighbor in GetNeighbors(current.Checkpoint.Position))
            {
                var newCost = current.Cost + 1;
                if (nodes.TryGetValue(neighbor.Position, out var value) && newCost >= value.Cost)
                    continue;
                
                var neighborNode = new Node(newCost, current, new Checkpoint(neighbor.Position, neighbor.Direction));
                nodes[neighbor.Position] = neighborNode;
                priorityQueue.Enqueue(neighborNode, newCost);
            }
        }

        return [];
    }

    private List<Checkpoint> ReconstructPath(Node endNode)
    {
        var path = new List<Checkpoint>();
        var current = endNode;
        
        while (current.Parent != null)
        {
            path.Add(current.Checkpoint);
            current = current.Parent;
        }
        
        path.Reverse();
        return path;
    }

    private List<Checkpoint> GetNeighbors(Point point)
    {
        var neighbors = new List<Checkpoint>
        {
            new(point with { X = point.X + 1 }, Direction.Right),
            new(point with { X = point.X - 1 }, Direction.Left),
            new(point with { Y = point.Y + 1 }, Direction.Forward),
            new(point with { Y = point.Y - 1 }, Direction.Back)
        };
        
        return neighbors.Where(neighbor =>
                map.FindIntersect(new RectangleF(neighbor.Position, new SizeF(box.Width, box.Height))) == null)
            .ToList();
    }

    private class Node(int cost, Node? parent, Checkpoint checkpoint)
    {
        public readonly int Cost = cost;
        public readonly Node? Parent  = parent;
        public readonly Checkpoint Checkpoint = checkpoint;
    }
}

public enum Direction
{
    Forward,
    Back,
    Left,
    Right,
    None
}

public record Checkpoint(Point Position, Direction Direction);
