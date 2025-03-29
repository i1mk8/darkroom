using darkroom.model;

namespace Darkroom.Tests;

[TestClass]
public class PlayerTests
{
    
    [DataTestMethod]
    [DataRow(2, 2, new[] { 1, 2 })]
    [DataRow(2, 4, new[] { 1, 2 })]
    [DataRow(10000, 10000, new[] { 1, 2 })]
    [DataRow(10000, 20000, new[] { 1, 2 })]
    [DataRow(2, 2, new[] { 0, 0 })]
    [DataRow(2, 2, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
    public void Test(int width, int height, int[] moves)
    {
        var player = new Player(width, height);

        var rows = moves.GetUpperBound(0) + 1;
        for (var i = 0; i < rows; i += 2)
        {
            player.MoveTo(moves[i], moves[i + 1]);
            
            CheckPlayerSize(player, width, height);
            CheckPlayerMovement(player, moves[i], moves[i + 1]);
        }
    }

    private void CheckPlayerMovement(Player player, int expectedX, int expectedY)
    {
        Assert.AreEqual(expectedX, player.Box.X);
    }

    private void CheckPlayerSize(Player player, int expectedWidth, int expectedHeight)
    {
        Assert.AreEqual(expectedWidth, player.Box.Width);
        Assert.AreEqual(expectedHeight, player.Box.Height);
    }
}