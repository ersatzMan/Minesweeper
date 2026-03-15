using MinesweeperChallenge.Hosting;
using MinesweeperChallenge.Enums;
using MinesweeperChallenge.Interfaces;
using Moq;

namespace MinesweeperChallenge.Tests;

[TestClass]
public sealed class TurnBasedGameHostTests
{
    private readonly Mock<IConsoleInput> _console = new();
    private readonly Mock<ITurnBasedGame> _game = new();
    private readonly Mock<ITurnBasedRenderer> _renderer = new();

    private TurnBasedGameHost? _sut;

    [TestInitialize]
    public void Setup() => _sut = new(_game.Object, _renderer.Object, _console.Object);

    [TestMethod]
    public void Run_CallsBuild_ThenPlaysUntilGameIsNoLongerInProgress()
    {
        // Arrange
        var grid = new ICell[1, 1];

        _game.Setup(g => g.Build()).Returns(grid);

        _game.SetupSequence(g => g.GetState())
            .Returns(GameState.InProgress)
            .Returns(GameState.InProgress)
            .Returns(GameState.Won);      

        _game.Setup(g => g.Play()).Returns(grid);

        // provide 'N' to the host loop so it doesn't attempt another replay
        _console.Setup(c => c.ReadKey()).Returns(new ConsoleKeyInfo('N', ConsoleKey.N, false, false, false));

        // Act
        _sut!.Run();

        // Assert
        _game.Verify(g => g.Build(), Times.Once);
        _game.Verify(g => g.Play(), Times.Exactly(2));
        _renderer.Verify(r => r.Render(grid), Times.Exactly(3));
    }
}