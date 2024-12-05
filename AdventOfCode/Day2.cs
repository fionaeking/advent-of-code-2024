namespace AdventOfCode;

public class Day2(string inputFilename) : IDay
{
    public void Part1()
    {
        var input = File.ReadAllLines(inputFilename)
            .Select(line => line.Split(" ").Select(int.Parse).ToArray());
        var count = input.Count(IsReportSafePartOne);
        Console.WriteLine(count);
    }

    private static bool IsReportSafePartOne(int[] line)
    {
        var diffs = GetDiffs(line).ToList();
        return ((diffs.All(d => Math.Abs(d) >= 1 && Math.Abs(d) <= 3)) &&
                (diffs.All(d => d > 0) || diffs.All(d => d < 0)));
    }

    private static IEnumerable<int> GetDiffs(int[] line)
    {
        return line.Take(line.Length - 1)
            .Select((v, i) => line[i + 1] - v);
    }

    private static bool IsReportSafePartTwo(int[] line)
    {
        var allOptions = new List<int[]>() { line };
        // For each element, create version of list with it removed
        for (var i = 0; i < line.Length; i++)
        {
            var newList = line.ToList();
            newList.RemoveAt(i);
            allOptions.Add(newList.ToArray());
        }
        return allOptions.Any(IsReportSafePartOne);
    }

    public void Part2()
    {
        var input = File.ReadAllLines(inputFilename)
            .Select(line => line.Split(" ").Select(int.Parse).ToArray());
        var count = input.Count(IsReportSafePartTwo);
        Console.WriteLine(count);
    }
}
