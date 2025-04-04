using System.Drawing;
using darkroom.model;

namespace Darkroom.Tests;

[TestClass]
public sealed class MapTests
{
    
    [DataTestMethod]
    [DataRow(100, 100, 1, 5, 10)]
    [DataRow(10000, 10000, 1, 5, 10)]
    [DataRow(100, 200, 5, 1, 10)]
    [DataRow(10000, 20000, 1, 5, 10)]
    [DataRow(100, 100, 5, 1, 2)]
    [DataRow(100, 100, 5, 99, 100)]
    [DataRow(100, 100, 100, 5, 10)]
    public void TestGeneration(int width, int height, int wallOffset, int minWallSize, int maxWallSize)
    {
        var map = Map.Generate(width, height, wallOffset, minWallSize, maxWallSize);
        
        CheckMapSize(map, width, height);
        CheckWallSize(map, minWallSize, maxWallSize);
    }

    private void CheckMapSize(Map map, int expectedWidth, int expectedHeight)
    {
        Assert.AreEqual(expectedWidth, map.Width);
        Assert.AreEqual(expectedHeight, map.Height);
    }

    private void CheckWallSize(Map map, int minWallSize, int maxWallSize)
    {
        Assert.IsTrue(map.Walls.All(w => w.Width <= maxWallSize && w.Height <= maxWallSize));
        Assert.IsTrue(map.Walls.Any(w => w.Width >= minWallSize || w.Height >= minWallSize));
    }

    [TestMethod]
    public void TestSuccessWithin()
    {
        var flag = false;
        var box = new RectangleF(50, 50, 1, 1);

        for (var i = 0; i < 100; i++)
        {
            var map = Map.Generate(100, 100, 1, 3, 5);
            if (!map.IsWithin(box))
                continue;
            
            flag = true;
            break;
        }

        Assert.IsTrue(flag);
    }

    [TestMethod]
    public void TestFailWithin()
    {
        var map = Map.Generate(100, 100, 1, 3, 5);
        var box1 = new RectangleF(-1, -1, 1, 1);
        var box2 = new RectangleF(101, 101, 1, 1);
        
        CheckFailWithin(map, box1);
        CheckFailWithin(map, box2);
    }

    private void CheckFailWithin(Map map, RectangleF box)
    {
        for (var i = 0; i < 100; i++)
            Assert.IsFalse(map.IsWithin(box));
    }
}