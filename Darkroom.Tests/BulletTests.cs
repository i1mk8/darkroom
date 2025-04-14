using darkroom.model;
using darkroom.UI.sound;

namespace Darkroom.Tests;

[TestClass]
public class BulletTests
{
    [TestMethod]
    public void MoveTo()
    {
        var map = new Map(50, 50, []);
        var player = new Player(map, 1f, 1f, 0.2f);
        var bulletProcessor = new BulletProcessor(map);
        player.Initialize(bulletProcessor, new SoundController(player));
            
        var bullet = new Bullet(player, 0.5f, 0.5f, 10f);
        var originalBox = bullet.Box;
        
        bullet.MoveTo(10f, 20f);
        
        Assert.AreEqual(10f, bullet.Box.X);
        Assert.AreEqual(20f, bullet.Box.Y);
        Assert.AreEqual(originalBox.Width, bullet.Box.Width);
        Assert.AreEqual(originalBox.Height, bullet.Box.Height);
    }
}