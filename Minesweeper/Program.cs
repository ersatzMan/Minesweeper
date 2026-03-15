using Microsoft.Extensions.DependencyInjection;
using Minesweeper.Games.Minesweeper;
using Minesweeper.Games.Minewalker;
using Minesweeper.Hosting;
using Minesweeper.Interfaces;

var serviceProvider = new ServiceCollection()
    .AddScoped<TurnBasedGameHost>()
    .AddScoped<ITurnBasedGame, MinewalkerGame>()
    .AddScoped<ITurnBasedRenderer, MinewalkerRenderer>()
    //.AddScoped<ITurnBasedGame, MinesweeperGame>()
    //.AddScoped<ITurnBasedRenderer, FancierMinesweeperRenderer>()
    .AddScoped<IConsoleInput, ConsoleInput>()
    .BuildServiceProvider();

serviceProvider.GetService<TurnBasedGameHost>()!.Run();