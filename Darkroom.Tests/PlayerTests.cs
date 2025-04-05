using darkroom.model;
using System.Drawing;

namespace darkroom.tests
{
    [TestClass]
    public class PlayerTests
    {
        private Map _map;
        private Player _player;

        [TestInitialize]
        public void Initialize()
        {
            _map = new Map(100, 100, new List<RectangleF>());
            _player = new Player(_map, 10, 10, 5);
        }

        [TestMethod]
        public void MoveTo_ValidPosition_ReturnsNullAndUpdatesBox()
        {
            var x = 50;
            var y = 50;
            var expectedBox = new RectangleF(x, y, 10, 10);
            
            var result = _player.MoveTo(x, y);
            
            Assert.IsNull(result);
            Assert.AreEqual(expectedBox, _player.Box);
        }

        [TestMethod]
        public void MoveTo_OutOfBoundsLeft_ReturnsLeftWall()
        {
            var result = _player.MoveTo(-5, 50);
            
            Assert.IsNotNull(result);
            Assert.AreEqual(new RectangleF(0, 0, 0, 100), result.Value);
            Assert.AreEqual(new RectangleF(-1, -1, 10, 10), _player.Box);
        }

        [TestMethod]
        public void MoveTo_OutOfBoundsRight_ReturnsRightWall()
        {
            var result = _player.MoveTo(95, 50);
            
            Assert.IsNotNull(result);
            Assert.AreEqual(new RectangleF(100, 0, 0, 100), result.Value);
            Assert.AreEqual(new RectangleF(-1, -1, 10, 10), _player.Box);
        }

        [TestMethod]
        public void MoveTo_OutOfBoundsTop_ReturnsTopWall()
        {
            var result = _player.MoveTo(50, -5);
            
            Assert.IsNotNull(result);
            Assert.AreEqual(new RectangleF(0, 0, 100, 0), result.Value);
            Assert.AreEqual(new RectangleF(-1, -1, 10, 10), _player.Box);
        }

        [TestMethod]
        public void MoveTo_OutOfBoundsBottom_ReturnsBottomWall()
        {
            var result = _player.MoveTo(50, 95);
            
            Assert.IsNotNull(result);
            Assert.AreEqual(new RectangleF(0, 100, 100, 0), result.Value);
            Assert.AreEqual(new RectangleF(-1, -1, 10, 10), _player.Box);
        }

        [TestMethod]
        public void MoveTo_IntersectsWithWall_ReturnsWall()
        {
            var wall = new RectangleF(30, 30, 10, 10);
            var mapWithWall = new Map(100, 100, [wall]);
            var player = new Player(mapWithWall, 10, 10, 5);
            
            var result = player.MoveTo(35, 35);
            
            Assert.IsNotNull(result);
            Assert.AreEqual(wall, result.Value);
            Assert.AreEqual(new RectangleF(-1, -1, 10, 10), player.Box);
        }

        [TestMethod]
        public void MoveForward_NoObstacle_MovesPlayer()
        {
            _player.MoveTo(50, 50);
            var expectedBox = new RectangleF(50, 55, 10, 10);
            
            _player.MoveForward();
            
            Assert.AreEqual(expectedBox, _player.Box);
        }

        [TestMethod]
        public void MoveForward_WithObstacle_StopsAtObstacle()
        {
            var wall = new RectangleF(50, 60, 10, 10);
            var mapWithWall = new Map(100, 100, [wall]);
            var player = new Player(mapWithWall, 10, 10, 5);
            player.MoveTo(50, 50);
            var expectedBox = new RectangleF(50, 50, 10, 10);
            
            player.MoveForward();
            
            Assert.AreEqual(expectedBox, player.Box);
        }

        [TestMethod]
        public void MoveBack_NoObstacle_MovesPlayer()
        {
            _player.MoveTo(50, 50);
            var expectedBox = new RectangleF(50, 45, 10, 10);
            
            _player.MoveBack();
            
            Assert.AreEqual(expectedBox, _player.Box);
        }

        [TestMethod]
        public void MoveBack_WithObstacle_StopsAtObstacle()
        {
            var wall = new RectangleF(50, 40, 10, 10);
            var mapWithWall = new Map(100, 100, [wall]);
            var player = new Player(mapWithWall, 10, 10, 5);
            player.MoveTo(50, 50);
            var expectedBox = new RectangleF(50, 50, 10, 10);
            
            player.MoveBack();
            
            Assert.AreEqual(expectedBox, player.Box);
        }

        [TestMethod]
        public void MoveRight_NoObstacle_MovesPlayer()
        {
            _player.MoveTo(50, 50);
            var expectedBox = new RectangleF(55, 50, 10, 10);
            
            _player.MoveRight();
            
            Assert.AreEqual(expectedBox, _player.Box);
        }

        [TestMethod]
        public void MoveRight_WithObstacle_StopsAtObstacle()
        {
            var wall = new RectangleF(60, 50, 10, 10);
            var mapWithWall = new Map(100, 100, [wall]);
            var player = new Player(mapWithWall, 10, 10, 5);
            player.MoveTo(50, 50);
            var expectedBox = new RectangleF(50, 50, 10, 10);
            
            player.MoveRight();
            
            Assert.AreEqual(expectedBox, player.Box);
        }

        [TestMethod]
        public void MoveLeft_NoObstacle_MovesPlayer()
        {
            _player.MoveTo(50, 50);
            var expectedBox = new RectangleF(45, 50, 10, 10);
            
            _player.MoveLeft();

            Assert.AreEqual(expectedBox, _player.Box);
        }

        [TestMethod]
        public void MoveLeft_WithObstacle_StopsAtObstacle()
        {
            var wall = new RectangleF(40, 50, 10, 10);
            var mapWithWall = new Map(100, 100, [wall]);
            var player = new Player(mapWithWall, 10, 10, 5);
            player.MoveTo(50, 50);
            var expectedBox = new RectangleF(50, 50, 10, 10);
            
            player.MoveLeft();
            
            Assert.AreEqual(expectedBox, player.Box);
        }

        [TestMethod]
        public void SpawnPlayer_PlacesPlayerInValidPosition()
        {
            var map = new Map(100, 100, []);
            var player = new Player(map, 10, 10, 5);
            
            player.SpawnPlayer();
            
            Assert.IsTrue(player.Box.Left >= 0);
            Assert.IsTrue(player.Box.Right <= map.Width);
            Assert.IsTrue(player.Box.Top >= 0);
            Assert.IsTrue(player.Box.Bottom <= map.Height);
        }

        [TestMethod]
        public void SpawnPlayer_WithWalls_PlacesPlayerInValidPosition()
        {
            var walls = new List<RectangleF> { new(20, 20, 60, 60) };
            var map = new Map(100, 100, walls);
            var player = new Player(map, 10, 10, 5);
            
            player.SpawnPlayer();
            
            Assert.IsFalse(player.Box.IntersectsWith(walls[0]));
            Assert.IsTrue(player.Box.Left >= 0);
            Assert.IsTrue(player.Box.Right <= map.Width);
            Assert.IsTrue(player.Box.Top >= 0);
            Assert.IsTrue(player.Box.Bottom <= map.Height);
        }
    }
}