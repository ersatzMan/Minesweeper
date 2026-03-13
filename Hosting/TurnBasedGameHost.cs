using MinesweeperChallenge.Enums;
using MinesweeperChallenge.Interfaces;

namespace MinesweeperChallenge.Hosting;

/// <summary>
/// Host consuming an implementation of <see cref="ITurnBasedGame"/>, encapsulating a simple turn-based game flow.
/// </summary>
public class TurnBasedGameHost(ITurnBasedGame game, ITurnBasedRenderer renderer) 
{
    public void Run()
    {
        do //Outer loop allowing for replays of the same game.
        {
            //Initialise the game.
            var grid = game.Build();
            renderer.Render(grid);                     

            //Loop until you win or lose
            while (game.GetState() == GameState.InProgress)
            {
                grid = game.Play();            //Read player input, print output (if any) and react accordingly.
                renderer.Render(grid);  //Draw the game field for a turn.
            }

            //Draw the game's end state.
            Console.WriteLine(game.GameOverMessage);
            Console.WriteLine("Press any key to play again.");
        } while (Console.ReadKey().KeyChar != 'N');
    }
}