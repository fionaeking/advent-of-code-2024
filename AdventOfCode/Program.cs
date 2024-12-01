using System.Diagnostics;

namespace AdventOfCode;

internal class Program
{
    private static void Main()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var day = new Day1("PuzzleInput.txt");
        day.Part1();
        day.Part2();

        stopwatch.Stop();
        var elapsedTime = stopwatch.Elapsed;
        Console.WriteLine("Elapsed time in ms: " + elapsedTime.Milliseconds);
    }
}