using Minesweeper.Interfaces;

namespace Minesweeper.Extensions;

public static class ValidationExtensions
{
    public static T PromptKeyPressUntilValid<T>(string prompt, Func<char, T> validator, IConsoleInput console) => Validate(prompt, _ => validator(console.ReadKey().KeyChar));
    public static T PromptConsoleKeyPressUntilValid<T>(string prompt, Func<ConsoleKey, T> validator, IConsoleInput console) => Validate(prompt, _ => validator(console.ReadKey().Key));
    public static T PromptCommandUntilValid<T>(string prompt, Func<string, T> validator, IConsoleInput console) => Validate(prompt, _ => validator(console.ReadLine()!));

    private static TU Validate<T, TU>(T prompt, Func<T, TU> validator)
    {
        TU result;

        var first = true;

        do
        {
            if (first)
                Console.WriteLine(prompt);
            result = validator(prompt);
            first = false;
        } while (result == null);

        return result;
    }
}