namespace darkroom.model;

public class PathFinder(Map map, RectangleF box)
{
    public List<Point> FindPath(Point start, Point end)
    {
        var nodes = new Dictionary<Point, Node>();
        var priorityQueue = new PriorityQueue<Node, int>();
        var startNode = new Node(0, null, start);
        
        nodes.Add(start, startNode);
        priorityQueue.Enqueue(startNode, 0);

        while (priorityQueue.Count > 0)
        {
            var current = priorityQueue.Dequeue();
            if (current.Checkpoint == end)
                return ReconstructPath(current);

            foreach (var neighbor in GetNeighbors(current.Checkpoint))
            {
                var newCost = current.Cost + 1;
                if (nodes.TryGetValue(neighbor, out var value) && newCost >= value.Cost)
                    continue;
                
                var neighborNode = new Node(newCost, current, neighbor);
                nodes[neighbor] = neighborNode;
                priorityQueue.Enqueue(neighborNode, newCost);
            }
        }

        return [];
    }

    private List<Point> ReconstructPath(Node endNode)
    {
        var path = new List<Point>();
        var current = endNode;
        
        while (current.Parent != null)
        {
            path.Add(current.Checkpoint);
            current = current.Parent;
        }
        
        path.Add(current.Checkpoint);
        path.Reverse();
        
        return path;
    }

    private List<Point> GetNeighbors(Point point)
    {
        var neighbors = new List<Point>
        {
            point with { X = point.X + 1 },
            point with { X = point.X - 1 },
            point with { Y = point.Y + 1 },
            point with { Y = point.Y - 1 },
        };
        
        return neighbors.Where(neighbor =>
                map.FindIntersect(new RectangleF(neighbor, new SizeF(box.Width, box.Height))) == null)
            .ToList();
    }

    private class Node(int cost, Node? parent, Point checkpoint)
    {
        public readonly int Cost = cost;
        public readonly Node? Parent  = parent;
        public readonly Point Checkpoint = checkpoint;
    }
}
