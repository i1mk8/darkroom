using System.Drawing;
using darkroom.model;

namespace Darkroom.Tests;

[TestClass]
public class MapTests
{
    [TestMethod]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        var width = 100;
        var height = 200;
        var walls = new List<RectangleF> { new(10, 10, 20, 20), new(50, 50, 30, 10) };
        
        var map = new Map(width, height, walls);
        
        Assert.AreEqual(width, map.Width);
        Assert.AreEqual(height, map.Height);
        CollectionAssert.AreEqual(walls, map.Walls);
    }

    [TestMethod]
    public void Generate_CreatesMapWithCorrectDimensions()
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
    }

    [TestMethod]
    public void Generate_WallsAreWithinMapBounds()
    {
        var width = 800;
        var height = 600;
        var wallOffset = 20;
        var minWallSize = 30;
        var maxWallSize = 100;
        
        var map = Map.Generate(width, height, wallOffset, minWallSize, maxWallSize);
        
        foreach (var wall in map.Walls)
        {
            Assert.IsTrue(wall.Left >= 0);
            Assert.IsTrue(wall.Top >= 0);
            Assert.IsTrue(wall.Right <= width);
            Assert.IsTrue(wall.Bottom <= height);
        }
    }

    [TestMethod]
    public void FindIntersect_ReturnsLeftBorderWhenObjectExceedsLeftBound()
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
    public void FindIntersect_ReturnsRightBorderWhenObjectExceedsRightBound()
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
    public void FindIntersect_ReturnsTopBorderWhenObjectExceedsTopBound()
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
    public void FindIntersect_ReturnsBottomBorderWhenObjectExceedsBottomBound()
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
    public void FindIntersect_ReturnsWallWhenObjectIntercetsWithWall()
    {
        var walls = new List<RectangleF> { new(50, 50, 20, 20) };
        var map = new Map(100, 100, walls);
        var box = new RectangleF(55, 55, 10, 10);
        
        var result = map.FindIntersect(box);
        
        Assert.IsNotNull(result);
        Assert.AreEqual(walls[0], result.Value);
    }

    [TestMethod]
    public void FindIntersect_ReturnsNullWhenNoIntercetions()
    {
        var walls = new List<RectangleF> { new(50, 50, 20, 20) };
        var map = new Map(100, 100, walls);
        var box = new RectangleF(10, 10, 20, 20);
        
        var result = map.FindIntersect(box);
        
        Assert.IsNull(result);
    }

    [TestMethod]
    public void Generate_ProducesDifferentWallConfigurations()
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
    public void Generate_RespectsMinAndMaxWallSizeConstraints()
    {
        var width = 800;
        var height = 600;
        var wallOffset = 20;
        var minWallSize = 30;
        var maxWallSize = 100;
        
        var map = Map.Generate(width, height, wallOffset, minWallSize, maxWallSize);
        
        foreach (var wall in map.Walls)
        {
            var size = wall.Width > wall.Height ? wall.Width : wall.Height;
            Assert.IsTrue(size >= minWallSize || size <= maxWallSize, 
                $"Размер стены {size} вне указанного диапазона [{minWallSize}, {maxWallSize}]");
        }
    }
}