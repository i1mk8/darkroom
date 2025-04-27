namespace darkroom.model;

/// <summary>
/// Строит путь для бота
/// </summary>
/// <param name="map">Игровоая карта</param>
/// <param name="box">Бокс бота</param>
public class PathFinder(Map map, RectangleF box)
{
    
    /// <summary>
    /// Ищет путь из одной точки в другую
    /// </summary>
    /// <param name="start">Начальная точка</param>
    /// <param name="end">Конечная точка</param>
    /// <returns>Путь из одной точки в другую</returns>
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
    
    /// <summary>
    /// Восстанавливает путь
    /// </summary>
    /// <param name="endNode">Конечный узел пути</param>
    /// <returns>Путь</returns>
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
    
    /// <summary>
    /// Получает ближайщих соседей, относительно точки
    /// </summary>
    /// <param name="point">Точка</param>
    /// <returns>Ближашие соседи</returns>
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
    
    /// <summary>
    /// Узел пути
    /// </summary>
    /// <param name="cost">Цена</param>
    /// <param name="parent">Родитель</param>
    /// <param name="checkpoint">Точка</param>
    private class Node(int cost, Node? parent, Point checkpoint)
    {
        public readonly int Cost = cost;
        public readonly Node? Parent  = parent;
        public readonly Point Checkpoint = checkpoint;
    }
}
