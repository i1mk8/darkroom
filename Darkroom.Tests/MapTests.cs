using System.Drawing;
using darkroom.model;

namespace Darkroom.Tests;

[TestClass]
public class MapTests
{

    [TestMethod]
    public void Generate()
    {
        var width = 800;
        var height = 600;
        var wallOffset = 20;
        var minWallSize = 30;
        var maxWallSize = 100;
        
        var map = Map.Generate(width, height, wallOffset, minWallSize, maxWallSize);
        
        Assert.AreEqual(width, map.Width);
        Assert.AreEqual(height, map.Height);
        Assert.IsTrue(map.Walls.Count > 0);

        foreach (var wall in map.Walls)
        {
            Assert.IsTrue(wall.Left >= 0 && wall.Top >= 0 && wall.Right <= width && wall.Bottom <= height);
            var size = wall.Width > wall.Height ? wall.Width : wall.Height;
            Assert.IsTrue(size >= minWallSize || size <= maxWallSize);
        }
    }
    
    [TestMethod]
    public void Generate_ReturnsRandomMaps()
    {
        var width = 800;
        var height = 600;
        var wallOffset = 20;
        var minWallSize = 30;
        var maxWallSize = 100;
        
        var map1 = Map.Generate(width, height, wallOffset, minWallSize, maxWallSize);
        var map2 = Map.Generate(width, height, wallOffset, minWallSize, maxWallSize);
        
        CollectionAssert.AreNotEqual(map1.Walls, map2.Walls);
    }

    [TestMethod]
    public void FindIntersect_WithLeftBorder()
    {
        var map = new Map(100, 100, []);
        var box = new RectangleF(-5, 10, 20, 20);
        
        var result = map.FindIntersect(box);
        
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Value.Left);
        Assert.AreEqual(0, result.Value.Top);
        Assert.AreEqual(0, result.Value.Right);
        Assert.AreEqual(100, result.Value.Bottom);
    }

    [TestMethod]
    public void FindIntersect_WithRightBorder()
    {
        var map = new Map(100, 100, []);
        var box = new RectangleF(95, 10, 20, 20);
        
        var result = map.FindIntersect(box);
        
        Assert.IsNotNull(result);
        Assert.AreEqual(100, result.Value.Left);
        Assert.AreEqual(0, result.Value.Top);
        Assert.AreEqual(100, result.Value.Right);
        Assert.AreEqual(100, result.Value.Bottom);
    }

    [TestMethod]
    public void FindIntersect_WithTopBorder()
    {
        var map = new Map(100, 100, []);
        var box = new RectangleF(10, -5, 20, 20);
        
        var result = map.FindIntersect(box);
        
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Value.Left);
        Assert.AreEqual(0, result.Value.Top);
        Assert.AreEqual(100, result.Value.Right);
        Assert.AreEqual(0, result.Value.Bottom);
    }

    [TestMethod]
    public void FindIntersect_WithBottomBorder()
    {
        var map = new Map(100, 100, []);
        var box = new RectangleF(10, 95, 20, 20);
        
        var result = map.FindIntersect(box);
        
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Value.Left);
        Assert.AreEqual(100, result.Value.Top);
        Assert.AreEqual(100, result.Value.Right);
        Assert.AreEqual(100, result.Value.Bottom);
    }

    [TestMethod]
    public void FindIntersect_WithWall()
    {
        var walls = new List<RectangleF> { new(50, 50, 20, 20) };
        var map = new Map(100, 100, walls);
        var box = new RectangleF(55, 55, 10, 10);
        
        var result = map.FindIntersect(box);
        
        Assert.IsNotNull(result);
        Assert.AreEqual(walls[0], result.Value);
    }

    [TestMethod]
    public void FindIntersect_WhenNoIntersections()
    {
        var walls = new List<RectangleF> { new(50, 50, 20, 20) };
        var map = new Map(100, 100, walls);
        var box = new RectangleF(10, 10, 20, 20);
        
        var result = map.FindIntersect(box);
        
        Assert.IsNull(result);
    }
}