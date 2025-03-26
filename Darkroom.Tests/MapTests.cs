using darkroom;

namespace Darkroom.Tests;

[TestClass]
public sealed class MapTests
{
    
    [DataTestMethod]
    [DataRow(100, 100, 5, 10)]
    [DataRow(10000, 10000, 5, 10)]
    [DataRow(100, 200, 5, 10)]
    [DataRow(10000, 20000, 5, 10)]
    [DataRow(100, 100, 1, 2)]
    [DataRow(100, 100, 98, 99)]
    public void Test(int width, int height, int minWallSize, int maxWallSize)
    {
        var map = Map.Generate(width, height, minWallSize, maxWallSize);
        
        CheckSize(map, width, height);
        Assert.IsFalse(IsEmpty(map));
    }

    private void CheckSize(Map map, int expectedWidth, int expectedHeight)
    {
        Assert.AreEqual(expectedWidth, map.Width);
        Assert.AreEqual(expectedHeight, map.Height);
    }

    private bool IsEmpty(Map map)
    {
        var empty = true;
        for (var x = 0; x < map.Width; x++)
            for (var y = 0; y < map.Height; y++)
                if (map.Field[x, y])
                {
                    empty = false;
                    break;
                }
        
        return empty;
    }
}