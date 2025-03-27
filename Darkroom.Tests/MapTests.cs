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
    public void Test(int width, int height, int wallOffset, int minWallSize, int maxWallSize)
    {
        var map = Map.Generate(width, height, wallOffset, minWallSize, maxWallSize);
        
        Assert.AreEqual(width, map.Width);
        Assert.AreEqual(height, map.Height);
        Assert.AreNotEqual(0, map.Walls.Count);
    }
}