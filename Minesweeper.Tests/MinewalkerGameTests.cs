using MinesweeperChallenge.Enums;
using MinesweeperChallenge.Extensions;
using MinesweeperChallenge.Games.Minewalker;
using MinesweeperChallenge.Interfaces;
using Moq;

namespace MinesweeperChallenge.Tests;

[TestClass]
public sealed class MinewalkerGameTests
{
    private MinewalkerGame? _sut;
    private readonly Mock<IConsoleInput> _console = new();

    [TestInitialize]
    public void Setup() => _sut = new(_console.Object);

    [TestMethod]
    public void Build_InitialGridHasCorrectDimensions()
    {
        var grid = _sut!.Build();

        Assert.IsNotNull(grid);

        Assert.AreEqual(0, grid.GetLowerBound(0));
        Assert.AreEqual(0, grid.GetLowerBound(1));
        Assert.AreEqual(7, grid.GetUpperBound(0));
        Assert.AreEqual(7, grid.GetUpperBound(1));

        var startCell = grid[0, 7];
        Assert.AreEqual('O', startCell.Draw());
    }

    [TestMethod]
    public void Build_CellsInitialisedWithCorrectValues()
    {
        var grid = _sut!.Build();

        //start cell
        Assert.AreEqual('O', grid[0, 7].Draw());

        //finish cell
        Assert.AreEqual('F', grid[7, 0].Draw());

        //the rest
        Assert.IsTrue(grid.ToArray().Select(cell => cell.Draw()).Where(ch => ch is not ('O' or 'F')).All(ch => ch == ' '));
    }

    [TestMethod]
    public void Build_Mines10Cells()
    {
        _sut!.Build();

        Assert.AreEqual(10, _sut.Grid.ToArray().Count(cell => cell.Mined));
    }

    /*Note: I ran out of time for the remaining tests, which were written by Github Copilot using the prompt:
        "Generate tests for the internal members of MinewalkerGame.cs, using [DataRow] where appropriate."
     */
    [TestMethod]
    [DataRow(ConsoleKey.LeftArrow, -1, -1)]
    [DataRow(ConsoleKey.UpArrow, 0, 6)]
    [DataRow(ConsoleKey.RightArrow, 1, 7)]
    [DataRow(ConsoleKey.DownArrow, -1, -1)]
    public void ValidateMove_ReturnsExpected(ConsoleKey key, int expectedX, int expectedY)
    {
        _sut!.Build();

        var result = _sut.ValidateMove(key);

        if (expectedX == -1 && expectedY == -1)
        {
            Assert.IsNull(result);
        }
        else
        {
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedX, result!.Value.x);
            Assert.AreEqual(expectedY, result.Value.y);
        }
    }

    [TestMethod]
    public void MovePlayerTo_UpdatesPositionAndFlags()
    {
        _sut!.Build();

        Assert.AreEqual((0, 7), _sut.CurrentPosition);
        Assert.IsTrue(_sut.Grid[0, 7].HasPlayer);

        _sut.MovePlayerTo(1, 7);

        Assert.AreEqual((1, 7), _sut.CurrentPosition);
        Assert.IsFalse(_sut.Grid[0, 7].HasPlayer);
        Assert.IsTrue(_sut.Grid[1, 7].HasPlayer);
    }

    [TestMethod]
    public void MovePlayerTo_SteppingOnMine_DecrementsLivesAndExplodes()
    {
        _sut!.Build();

        var target = _sut.Grid[1, 7];
        target.Mine();

        var initialLives = _sut.Lives;
        Assert.IsTrue(target.Mined);
        Assert.AreEqual(' ', target.Draw());

        _sut.MovePlayerTo(1, 7);

        Assert.AreEqual(initialLives - 1, _sut.Lives);
        Assert.IsTrue(_sut.Grid[1, 7].HasPlayer);
        Assert.IsTrue(_sut.Grid[1, 7].Exploded);
    }

    [TestMethod]
    public void RevealAllMines_RevealsMinedCells()
    {
        _sut!.Build();

        // pick two arbitrary cells and mine them
        _sut.Grid[2, 2].Mine();
        _sut.Grid[3, 3].Mine();

        Assert.IsTrue(_sut.Grid[2, 2].Mined);
        Assert.IsTrue(_sut.Grid[3, 3].Mined);

        // before reveal they should render as ' ' (mined but not revealed)
        Assert.AreEqual(' ', _sut.Grid[2, 2].Draw());
        Assert.AreEqual(' ', _sut.Grid[3, 3].Draw());

        _sut.RevealAllMines();

        // after reveal they should render as '*' (mined and revealed)
        Assert.AreEqual('*', _sut.Grid[2, 2].Draw());
        Assert.AreEqual('*', _sut.Grid[3, 3].Draw());
    }

    //probably too brittle
    [TestMethod]
    [DataRow(GameState.Won, "Congratulations! you made it out alive!")]
    [DataRow(GameState.Lost, "Commiserations. You were blown to smithereens.")]
    public void GameOverMessage_ReturnsExpected(GameState state, string expectedMessage)
    {
        _sut!.State = state;
        Assert.AreEqual(expectedMessage, _sut.GameOverMessage);
    }
}