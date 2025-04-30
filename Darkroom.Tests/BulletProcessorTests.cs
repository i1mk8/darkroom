using System.Drawing;
using darkroom.model;
using darkroom.UI.sound;

namespace Darkroom.Tests;

[TestClass]
public class BulletProcessorTests
{
    [TestMethod]
    public void AddBullet()
    {
        var map = new Map(50, 50, []);
        var processor = new BulletProcessor(map);
        var player = new Player(map, 1f, 1f, 0.2f);
        var soundController = new SoundController(player);
        player.Initialize(processor, soundController);
        
        var bullet = new Bullet(player, 0.5f, 0.5f, 10f);
        
        processor.AddBullet(bullet);
        
        Assert.AreEqual(1, processor.Bullets.Count);
        Assert.AreEqual(bullet, processor.Bullets[0]);
    }

    [TestMethod]
    public void AddPlayer()
    {
        var map = new Map(50, 50, []);
        var processor = new BulletProcessor(map);
        var player = new Player(map, 1f, 1f, 0.2f);
        
        processor.AddPlayer(player);
        
        Assert.AreEqual(1, processor.Players.Count);
        Assert.AreEqual(player, processor.Players[0]);
    }

    [TestMethod]
    public void Process_MovesBulletInDirection()
    {
        var map = new Map(50, 50, []);
        var processor = new BulletProcessor(map);
        var player = new Player(map, 1f, 1f, 0.2f);
        var soundController = new SoundController(player);
        player.Initialize(processor, soundController);
        
        var bullet = new Bullet(player, 0.1f, 0.1f, 0.5f);
        var originalPosition = bullet.Box.Location;
        processor.AddBullet(bullet);
    
        processor.Process();
        
        var expectedX = originalPosition.X + bullet.Direction.X * bullet.Speed;
        var expectedY = originalPosition.Y + bullet.Direction.Y * bullet.Speed;
        Assert.AreEqual(expectedX, bullet.Box.X, 0.001f);
        Assert.AreEqual(expectedY, bullet.Box.Y, 0.001f);
    }

    [TestMethod]
    public void Process_RemovesBulletWhenHitsWall()
    {
        var walls = new List<RectangleF> { new(10, 10, 5, 5) };
        var map = new Map(50, 50, walls);
        var processor = new BulletProcessor(map);
        var player = new Player(map, 1f, 1f, 0.2f);
        var soundController = new SoundController(player);
        player.Initialize(processor, soundController);
        
        player.MoveTo(9, 11);
        var bullet = new Bullet(player, 0.5f, 0.5f, 0.5f);
        processor.AddBullet(bullet);
        
        processor.Process();
        
        Assert.AreEqual(0, processor.Bullets.Count);
    }
    
    [TestMethod]
    public void Process_RemovesBulletWhenHitsPlayer()
    {
        var map = new Map(50, 50, []);
        var processor = new BulletProcessor(map);
    
        var originPlayer = new Player(map, 1f, 1f, 0.2f);
        var soundController = new SoundController(originPlayer);
        originPlayer.Initialize(processor, soundController);
        originPlayer.MoveTo(0, 0);
    
        var target = new Player(map, 1f, 1f, 0.2f);
        target.Initialize(processor, new SoundController(originPlayer));
        target.MoveTo(1f, 0);
    
        var bullet = new Bullet(originPlayer, 0.1f, 0.1f, 0.5f);
        processor.AddBullet(bullet);
        
        processor.Process();
    
        Assert.AreEqual(0, processor.Bullets.Count);
    }

    [TestMethod]
    public void Process_DoesNotHitOriginPlayer()
    {
        var map = new Map(50, 50, []);
        var processor = new BulletProcessor(map);
        
        var player = new Player(map, 1f, 1f, 0.2f);
        var soundController = new SoundController(player);
        player.Initialize(processor, soundController);
        player.MoveTo(0, 0);
        
        var bullet = new Bullet(player, 0.5f, 0.5f, 10f);
        processor.AddBullet(bullet);
        
        processor.Process();
        
        Assert.AreEqual(1, processor.Bullets.Count);
    }

    [TestMethod]
    public void Process_RemovesBulletWhenOutOfMapBounds()
    {
        var map = new Map(50, 50, []);
        var processor = new BulletProcessor(map);
    
        var player = new Player(map, 1f, 1f, 0.2f);
        var soundController = new SoundController(player);
        player.Initialize(processor, soundController);
        player.MoveTo(49, 49);
        
        var bullet = new Bullet(player, 0.1f, 0.1f, 0.5f);
        processor.AddBullet(bullet);
    
        processor.Process();
    
        Assert.AreEqual(0, processor.Bullets.Count);
    }
}