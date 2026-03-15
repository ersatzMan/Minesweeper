using Microsoft.Extensions.DependencyInjection;
using MinesweeperChallenge.Games.Minesweeper;
using MinesweeperChallenge.Games.Minewalker;
using MinesweeperChallenge.Hosting;
using MinesweeperChallenge.Interfaces;

var serviceProvider = new ServiceCollection()
    .AddScoped<TurnBasedGameHost>()
    .AddScoped<ITurnBasedGame, MinewalkerGame>()
    .AddScoped<ITurnBasedRenderer, MinewalkerRenderer>()
    //.AddScoped<ITurnBasedGame, MinesweeperGame>()
    //.AddScoped<ITurnBasedRenderer, FancierMinesweeperRenderer>()
    .AddScoped<IConsoleInput, ConsoleInput>()
    .BuildServiceProvider();

serviceProvider.GetService<TurnBasedGameHost>()!.Run();