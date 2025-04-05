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
        var walls = new List<RectangleF> { new(10, 10, 20, 20) };
            
        var map = new Map(width, height, walls);
            
        Assert.AreEqual(width, map.Width);
        Assert.AreEqual(height, map.Height);
        Assert.AreEqual(walls, map.Walls);
    }

    [TestMethod]
    public void Generate_WithValidParameters_ReturnsMapWithWalls()
    {
        var width = 100;
        var height = 100;
        var wallOffset = 5;
        var minWallSize = 10;
        var maxWallSize = 20;
            
        var map = Map.Generate(width, height, wallOffset, minWallSize, maxWallSize);
            
        Assert.IsNotNull(map);
        Assert.AreEqual(width, map.Width);
        Assert.AreEqual(height, map.Height);
        Assert.IsTrue(map.Walls.Count > 0);
    }

    [TestMethod]
    public void Generate_WallsDoNotExceedMapBoundaries()
    {
        var width = 100;
        var height = 100;
        var wallOffset = 5;
        var minWallSize = 10;
        var maxWallSize = 20;
            
        var map = Map.Generate(width, height, wallOffset, minWallSize, maxWallSize);
            
        foreach (var wall in map.Walls)
        {
            Assert.IsTrue(wall.Left >= 0 && wall.Right <= width, "Стена выходит за горизонтальные границы карты");
            Assert.IsTrue(wall.Top >= 0 && wall.Bottom <= height, "Стена выходит за вертикальные границы карты");
        }
    }

    [TestMethod]
    public void IsWithin_ReturnsTrueForObjectInsideMapAndOutsideWalls()
    {
        var width = 100;
        var height = 100;
        var walls = new List<RectangleF> { new(20, 20, 30, 30) };
        var map = new Map(width, height, walls);
        var testBox = new RectangleF(5, 5, 10, 10);
            
        var result = map.IsWithin(testBox);
            
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsWithin_ReturnsFalseForObjectOutsideMap()
    {
        var width = 100;
        var height = 100;
        var walls = new List<RectangleF>();
        var map = new Map(width, height, walls);
            
        var testBoxOutsideRight = new RectangleF(101, 0, 10, 10);
        var testBoxOutsideBottom = new RectangleF(0, 101, 10, 10);
        var testBoxOutsideLeft = new RectangleF(-5, 0, 10, 10);
        var testBoxOutsideTop = new RectangleF(0, -5, 10, 10);
            
        Assert.IsFalse(map.IsWithin(testBoxOutsideRight));
        Assert.IsFalse(map.IsWithin(testBoxOutsideBottom));
        Assert.IsFalse(map.IsWithin(testBoxOutsideLeft));
        Assert.IsFalse(map.IsWithin(testBoxOutsideTop));
    }

    [TestMethod]
    public void IsWithin_ReturnsFalseForObjectIntersectingWithWall()
    {
        var width = 100;
        var height = 100;
        var wall = new RectangleF(20, 20, 30, 30);
        var map = new Map(width, height, [wall]);
            
        var testBoxInsideWall = new RectangleF(25, 25, 5, 5); // Полностью внутри стены
        var testBoxOverlappingWall = new RectangleF(15, 15, 10, 10); // Частичное пересечение
            
        Assert.IsFalse(map.IsWithin(testBoxInsideWall));
        Assert.IsFalse(map.IsWithin(testBoxOverlappingWall));
    }

    [TestMethod]
    public void Generate_WithWallSizeEqualToMap_CreatesSingleWall()
    {
        var width = 100;
        var height = 100;
        var wallOffset = 0;
        var minWallSize = 100;
        var maxWallSize = 100;
            
        var map = Map.Generate(width, height, wallOffset, minWallSize, maxWallSize);
            
        Assert.AreEqual(1, map.Walls.Count);
        Assert.IsTrue(map.Walls[0] == new RectangleF(0, 49, 100, 1) || map.Walls[0] == new RectangleF(49, 0, 1, 100));
    }
}