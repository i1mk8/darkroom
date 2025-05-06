using System.Drawing;
using darkroom.game;
using darkroom.game.bot;

namespace Darkroom.Tests;

[TestClass]
public class PathFinderTests
{

    [TestMethod]
    public void FindPath_NoObstacles()
    {
        var map = new Map(5, 5, []);
        var box = new RectangleF(0, 0, 1f, 1f);
        var pathFinder = new PathFinder(map, box);
        var start = new Point(0, 0);
        var end = new Point(2, 0);

        var path = pathFinder.FindPath(start, end);

        var expectedPath = new List<Point> { new(0, 0), new(1, 0), new(2, 0) };
        CollectionAssert.AreEqual(expectedPath, path);
    }

    [TestMethod]
    public void FindPath_WithObstacles()
    {
        var walls = new List<RectangleF> { new(1.5f, 0, 1, 3) };
        var map = new Map(5, 5, walls);
        var box = new RectangleF(0, 0, 1f, 1f);
        var pathFinder = new PathFinder(map, box);
        var start = new Point(0, 0);
        var end = new Point(3, 0);

        var path = pathFinder.FindPath(start, end);
        
        var expectedPath = new List<Point>
        {
            new(0, 0),
            new(0, 1),
            new(0, 2),
            new(0, 3),
            new(1, 3),
            new(2, 3),
            new(3, 3),
            new(3, 2),
            new(3, 1),
            new(3, 0)
        };
        CollectionAssert.AreEqual(expectedPath, path);
    }

    [TestMethod]
    public void FindPath_BlockedPath()
    {
        var walls = new List<RectangleF> { new(0, 0, 5, 5) };
        var map = new Map(5, 5, walls);
        var box = new RectangleF(0, 0, 1f, 1f);
        var pathFinder = new PathFinder(map, box);
        var start = new Point(0, 0);
        var end = new Point(3, 0);

        var path = pathFinder.FindPath(start, end);

        Assert.AreEqual(0, path.Count);
    }

    [TestMethod]
    public void FindPath_SameStartAndEnd()
    {
        var map = new Map(5, 5, []);
        var box = new RectangleF(0, 0, 1f, 1f);
        var pathFinder = new PathFinder(map, box);
        var point = new Point(2, 2);

        var path = pathFinder.FindPath(point, point);

        var expectedPath = new List<Point> { point };
        CollectionAssert.AreEqual(expectedPath, path);
    }

    [TestMethod]
    public void FindPath_DiagonalMovement()
    {
        var map = new Map(5, 5, []);
        var box = new RectangleF(0, 0, 1f, 1f);
        var pathFinder = new PathFinder(map, box);
        var start = new Point(0, 0);
        var end = new Point(2, 2);

        var path = pathFinder.FindPath(start, end);
        
        var expectedPath = new List<Point>
        {
            new(0, 0),
            new(1, 0),
            new(1, 1),
            new(1, 2),
            new(2, 2)
        };
        CollectionAssert.AreEqual(expectedPath, path);
    }
    
    [TestMethod]
    public void FindPath_OutOfBounds()
    {
        var map = new Map(5, 5, []);
        var box = new RectangleF(0, 0, 1f, 1f);
        var pathFinder = new PathFinder(map, box);
        var start = new Point(-1, -1);
        var end = new Point(6, 6);

        var path = pathFinder.FindPath(start, end);

        Assert.AreEqual(0, path.Count);
    }
}