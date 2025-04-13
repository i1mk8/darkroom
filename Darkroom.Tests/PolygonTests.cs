using System.Drawing;
using darkroom.utils;

namespace Darkroom.Tests;

[TestClass]
public class PolygonTests
{

    [TestMethod]
    public void Contains_Points()
    {
        var polygon = new Polygon([new PointF(0, 0),
            new PointF(10, 0),
            new PointF(10, 10),
            new PointF(0, 10),
            new PointF(0, 0)]);
        
        Assert.IsTrue(polygon.Contains(new PointF(5, 5)));
        Assert.IsFalse(polygon.Contains(new PointF(11, 5)));
    }

    [TestMethod]
    public void Contains_Objects()
    {
        var polygon = new Polygon([new PointF(0, 0),
            new PointF(10, 0),
            new PointF(10, 10),
            new PointF(0, 10),
            new PointF(0, 0)]);
        
        Assert.IsTrue(polygon.Contains(new RectangleF(2, 2, 2, 2)));
        Assert.IsTrue(polygon.Contains(new RectangleF(8, 8, 3, 3)));
        Assert.IsFalse(polygon.Contains(new RectangleF(11, 11, 3, 3)));
    }
    
}