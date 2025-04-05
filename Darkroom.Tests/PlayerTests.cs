using darkroom.model;
using System.Drawing;

namespace darkroom.tests
{
    [TestClass]
    public class PlayerTests
    {
        
        [TestMethod]
        public void MoveTo_ValidPosition()
        {
            var map = new Map(100, 100, []);
            var player = new Player(map, 10, 10, 5);
            var x = 50;
            var y = 50;
            
            var result = player.MoveTo(x, y);
            
            Assert.IsNull(result);
            Assert.AreEqual(new RectangleF(x, y, 10, 10), player.Box);
        }

        [TestMethod]
        public void MoveTo_OutOfBoundsLeft()
        {
            var map = new Map(100, 100, []);
            var player = new Player(map, 10, 10, 5);
            
            var result = player.MoveTo(-5, 50);
            
            Assert.IsNotNull(result);
            Assert.AreEqual(new RectangleF(0, 0, 0, 100), result.Value);
            Assert.AreEqual(new RectangleF(-1, -1, 10, 10), player.Box);
        }

        [TestMethod]
        public void MoveTo_OutOfBoundsRight()
        {
            var map = new Map(100, 100, []);
            var player = new Player(map, 10, 10, 5);
            
            var result = player.MoveTo(95, 50);
            
            Assert.IsNotNull(result);
            Assert.AreEqual(new RectangleF(100, 0, 0, 100), result.Value);
            Assert.AreEqual(new RectangleF(-1, -1, 10, 10), player.Box);
        }

        [TestMethod]
        public void MoveTo_OutOfBoundsTop()
        {            
            var map = new Map(100, 100, []);
            var player = new Player(map, 10, 10, 5);
            
            var result = player.MoveTo(50, -5);
            
            Assert.IsNotNull(result);
            Assert.AreEqual(new RectangleF(0, 0, 100, 0), result.Value);
            Assert.AreEqual(new RectangleF(-1, -1, 10, 10), player.Box);
        }

        [TestMethod]
        public void MoveTo_OutOfBoundsBottom()
        {
            var map = new Map(100, 100, []);
            var player = new Player(map, 10, 10, 5);
            
            var result = player.MoveTo(50, 95);
            
            Assert.IsNotNull(result);
            Assert.AreEqual(new RectangleF(0, 100, 100, 0), result.Value);
            Assert.AreEqual(new RectangleF(-1, -1, 10, 10), player.Box);
        }

        [TestMethod]
        public void MoveTo_IntersectsWithWall()
        {
            var wall = new RectangleF(30, 30, 10, 10);
            var map = new Map(100, 100, [wall]);
            var player = new Player(map, 10, 10, 5);
            
            var result = player.MoveTo(35, 35);
            
            Assert.IsNotNull(result);
            Assert.AreEqual(wall, result.Value);
            Assert.AreEqual(new RectangleF(-1, -1, 10, 10), player.Box);
        }

        [TestMethod]
        public void MoveForward_WithoutObstacle()
        {
            var map = new Map(100, 100, []);
            var player = new Player(map, 10, 10, 5);
            player.MoveTo(50, 50);
            
            player.MoveForward();
            
            Assert.AreEqual(new RectangleF(50, 55, 10, 10), player.Box);
        }

        [TestMethod]
        public void MoveForward_WithObstacle()
        {
            var wall = new RectangleF(50, 60, 10, 10);
            var map = new Map(100, 100, [wall]);
            var player = new Player(map, 10, 10, 5);
            player.MoveTo(50, 50);
            
            player.MoveForward();
            
            Assert.AreEqual(new RectangleF(50, 50, 10, 10), player.Box);
        }

        [TestMethod]
        public void MoveBack_WithoutObstacle()
        {
            var map = new Map(100, 100, []);
            var player = new Player(map, 10, 10, 5);
            player.MoveTo(50, 50);
            
            player.MoveBack();
            
            Assert.AreEqual(new RectangleF(50, 45, 10, 10), player.Box);
        }

        [TestMethod]
        public void MoveBack_WithObstacle()
        {
            var wall = new RectangleF(50, 40, 10, 10);
            var map = new Map(100, 100, [wall]);
            var player = new Player(map, 10, 10, 5);
            player.MoveTo(50, 50);
            
            player.MoveBack();
            
            Assert.AreEqual(new RectangleF(50, 50, 10, 10), player.Box);
        }

        [TestMethod]
        public void MoveRight_WithoutObstacle()
        {
            var map = new Map(100, 100, []);
            var player = new Player(map, 10, 10, 5);
            player.MoveTo(50, 50);
            
            player.MoveRight();
            
            Assert.AreEqual(new RectangleF(55, 50, 10, 10), player.Box);
        }

        [TestMethod]
        public void MoveRight_WithObstacle()
        {
            var wall = new RectangleF(60, 50, 10, 10);
            var map = new Map(100, 100, [wall]);
            var player = new Player(map, 10, 10, 5);
            player.MoveTo(50, 50);
            
            player.MoveRight();
            
            Assert.AreEqual(new RectangleF(50, 50, 10, 10), player.Box);
        }

        [TestMethod]
        public void MoveLeft_WithoutObstacle()
        {
            var map = new Map(100, 100, []);
            var player = new Player(map, 10, 10, 5);
            player.MoveTo(50, 50);
            
            player.MoveLeft();

            Assert.AreEqual(new RectangleF(45, 50, 10, 10), player.Box);
        }

        [TestMethod]
        public void MoveLeft_WithObstacle()
        {
            var wall = new RectangleF(40, 50, 10, 10);
            var map = new Map(100, 100, [wall]);
            var player = new Player(map, 10, 10, 5);
            player.MoveTo(50, 50);
            
            player.MoveLeft();
            
            Assert.AreEqual(new RectangleF(50, 50, 10, 10), player.Box);
        }

        [TestMethod]
        public void SpawnPlayer_WithoutWalls()
        {
            var map = new Map(100, 100, []);
            var player = new Player(map, 10, 10, 5);
            
            player.SpawnPlayer();
            
            Assert.IsTrue(player.Box.Left >= 0
                          && player.Box.Right <= map.Width
                          && player.Box.Top >= 0
                          && player.Box.Bottom <= map.Height);
        }

        [TestMethod]
        public void SpawnPlayer_WithWalls()
        {
            var walls = new List<RectangleF> { new(20, 20, 60, 60) };
            var map = new Map(100, 100, walls);
            var player = new Player(map, 10, 10, 5);
            
            player.SpawnPlayer();
            
            Assert.IsFalse(player.Box.IntersectsWith(walls[0]));
            Assert.IsTrue(player.Box.Left >= 0
                          && player.Box.Right <= map.Width
                          && player.Box.Top >= 0
                          && player.Box.Bottom <= map.Height);
        }
    }
}