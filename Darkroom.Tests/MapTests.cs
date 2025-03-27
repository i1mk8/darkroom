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
}