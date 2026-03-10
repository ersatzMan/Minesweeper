using Minesweeper;

do
{
    RunGame();
    Console.WriteLine(Game.PlayAgainPrompt);
} while (Console.ReadKey().KeyChar != 'N');

return;

static void RunGame()
{
    var game = new Game();
    Game.Build();

    while (game.State == Game.GameState.InProgress)
    {
        Game.Draw();

        game.Play(
            PromptUntilValid(game.PrintMovePrompt, game.ValidateMove)!,
            (int)PromptUntilValid(Game.ChooseXPrompt, c => Game.ValidateDimension(c))!,
            (int)PromptUntilValid(Game.ChooseYPrompt, c => Game.ValidateDimension(c, false))!
        );
    }

    Game.Draw();

    Console.WriteLine(game.ResultPrompt);
}

static T PromptUntilValid<T>(string prompt, Func<char, T> validator)
{
    T result;

    do
    {
        Console.WriteLine(prompt);
        result = validator(Console.ReadKey().KeyChar);
        Console.WriteLine();
    } while (result == null);

    return result;
}