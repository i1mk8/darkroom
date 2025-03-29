using darkroom.model;

namespace Darkroom.Tests;

[TestClass]
public class PlayerTests
{
    
    [DataTestMethod]
    [DataRow(2, 2, new float[] { 1, 2 })]
    [DataRow(2, 4, new float[] { 1, 2 })]
    [DataRow(10000, 10000, new float[] { 1, 2 })]
    [DataRow(10000, 20000, new float[] { 1, 2 })]
    [DataRow(2, 2, new[] { 1.1f, 2.2f })]
    [DataRow(2, 2, new float[] { 0, 0 })]
    [DataRow(2, 2, new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
    public void Test(int width, int height, float[] moves)
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

    private void CheckPlayerMovement(Player player, float expectedX, float expectedY)
    {
        Assert.AreEqual(expectedX, player.Box.X);
        Assert.AreEqual(expectedY, player.Box.Y);
    }

    private void CheckPlayerSize(Player player, int expectedWidth, int expectedHeight)
    {
        Assert.AreEqual(expectedWidth, player.Box.Width);
        Assert.AreEqual(expectedHeight, player.Box.Height);
    }
}