using darkroom.model;
using System.Drawing;

namespace darkroom.tests;

[TestClass]
public class PlayerTests
{
    [TestMethod]
    public void MoveTo_ValidPosition_ReturnsTrueAndUpdatesBox()
    {
        var map = new Map(100, 100, new List<RectangleF>());
        var player = new Player(10, 10, map);
        
        var result = player.MoveTo(50, 50);
        
        Assert.IsTrue(result);
        Assert.AreEqual(new RectangleF(50, 50, 10, 10), player.Box);
    }

    [TestMethod]
    public void MoveTo_OutsideMap_ReturnsFalse()
    {
        var map = new Map(100, 100, new List<RectangleF>());
        var player = new Player(10, 10, map);
        
        Assert.IsFalse(player.MoveTo(-10, 50)); // За левой границей
        Assert.IsFalse(player.MoveTo(50, -10)); // За верхней границей
        Assert.IsFalse(player.MoveTo(95, 50));  // За правой границей (95+10 > 100)
        Assert.IsFalse(player.MoveTo(50, 95));  // За нижней границей (95+10 > 100)
    }

    [TestMethod]
    public void MoveTo_IntoWall_ReturnsFalse()
    {
        var walls = new List<RectangleF> { new(50, 50, 20, 20) };
        var wallMap = new Map(100, 100, walls);
        var wallPlayer = new Player(10, 10, wallMap);
        
        Assert.IsFalse(wallPlayer.MoveTo(45, 45)); // Пересекается с стеной
        Assert.IsTrue(wallPlayer.MoveTo(30, 30));  // Не пересекается со стеной
    }

    [TestMethod]
    public void MoveForward_ValidMove_UpdatesPosition()
    {
        var map = new Map(100, 100, new List<RectangleF>());
        var player = new Player(10, 10, map);
        
        player.MoveTo(50, 50);
        var initialY = player.Box.Y;
        
        player.MoveForward();
        
        Assert.AreEqual(initialY + Player.Speed, player.Box.Y);
    }

    [TestMethod]
    public void MoveBack_ValidMove_UpdatesPosition()
    {
        var map = new Map(100, 100, new List<RectangleF>());
        var player = new Player(10, 10, map);
        
        player.MoveTo(50, 50);
        var initialY = player.Box.Y;
        
        player.MoveBack();
        
        Assert.AreEqual(initialY - Player.Speed, player.Box.Y);
    }

    [TestMethod]
    public void MoveRight_ValidMove_UpdatesPosition()
    {
        var map = new Map(100, 100, new List<RectangleF>());
        var player = new Player(10, 10, map);
        
        player.MoveTo(50, 50);
        var initialX = player.Box.X;
        
        player.MoveRight();
        
        Assert.AreEqual(initialX + Player.Speed, player.Box.X);
    }

    [TestMethod]
    public void MoveLeft_ValidMove_UpdatesPosition()
    {
        var map = new Map(100, 100, new List<RectangleF>());
        var player = new Player(10, 10, map);
        
        player.MoveTo(50, 50);
        var initialX = player.Box.X;
        
        player.MoveLeft();
        
        Assert.AreEqual(initialX - Player.Speed, player.Box.X);
    }

    [TestMethod]
    public void SpawnPlayer_PlacesPlayerWithinMapBounds()
    {
        var map = new Map(100, 100, new List<RectangleF>());
        var player = new Player(10, 10, map);
        
        player.SpawnPlayer();
        
        Assert.IsTrue(player.Box.X >= 0);
        Assert.IsTrue(player.Box.Y >= 0);
        Assert.IsTrue(player.Box.Right <= map.Width);
        Assert.IsTrue(player.Box.Bottom <= map.Height);
    }

    [TestMethod]
    public void SpawnPlayer_WithWalls_PlacesPlayerOutsideWalls()
    {
        var walls = new List<RectangleF> { new(40, 40, 20, 20) };
        var wallMap = new Map(100, 100, walls);
        var wallPlayer = new Player(10, 10, wallMap);
        
        wallPlayer.SpawnPlayer();
        
        Assert.IsFalse(walls[0].IntersectsWith(wallPlayer.Box));
    }

    [TestMethod]
    public void InitialPosition_IsInvalid()
    {
        var map = new Map(100, 100, new List<RectangleF>());
        var player = new Player(10, 10, map);
        
        Assert.AreEqual(new RectangleF(-1, -1, 10, 10), player.Box);
    }
}