using System.Drawing;
using darkroom.model;

namespace Darkroom.Tests;

[TestClass]
public class FovTests
{
    [TestMethod]
    public void GetFov_ReturnsClosedPolygon()
    {
        var map = new Map(100, 100, [new RectangleF(50, 50, 20, 20)]);
        var player = new Player(map, 10, 10, 5);
        player.MoveTo(50, 30);
        var fov = new Fov(map, player, 10f, 90f, 1);
        
        var polygon = fov.GetFov();
            
        Assert.AreEqual(polygon.Vertices[0], polygon.Vertices[^1]);
    }
    
    [TestMethod]
    public void GetFov_WithNoObstacles()
    {
        var map = new Map(100, 100, []);
        var player = new Player(map, 10, 10, 5);
        player.MoveTo(10, 10);
        var fov = new Fov(map, player, 15f, 90f, 1);
        
        var polygon = fov.GetFov();
            
        Assert.IsTrue(polygon.Contains(new PointF(20f, 15f)));
        Assert.IsFalse(polygon.Contains(new PointF(40f, 15f)));
    }
    
    [TestMethod]
    public void GetFov_WithObstacle()
    {
        var map = new Map(100, 100, [new RectangleF(50, 50, 20, 20)]);
        var player = new Player(map, 10, 10, 5);
        player.MoveTo(50, 30);
        var fov = new Fov(map, player, 20f, 90f, 1);
        
        var polygon = fov.GetFov();
            
        Assert.IsFalse(polygon.Contains(new PointF(55f, 50f)));
    }
}