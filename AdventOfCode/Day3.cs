using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day3(string inputFilename) : IDay
{
    // Regex that matches mul(X,Y) where X and Y are integers (1 to 3 digits long)
    private readonly Regex _mulRegex = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)");

    private readonly Regex _doRegex = new Regex(@"do\(\)");
    private readonly Regex _dontRegex = new Regex(@"don\'t\(\)");


    public void Part1()
    {
        var input = File.ReadAllText(inputFilename);
        var matches = _mulRegex.Matches(input);
        var sum = matches.Select(m => (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value)))
            .Select(t => t.Item1 * t.Item2)
            .Sum();
        Console.WriteLine(sum);
    }

    public void Part2()
    {
        var input = File.ReadAllText(inputFilename);

        var mulMatches = _mulRegex.Matches(input).OrderBy(m => m.Index);
        var doMatchIndexes = _doRegex.Matches(input).Select(m => m.Index).OrderByDescending(m => m);
        var dontMatchIndexes = _dontRegex.Matches(input).Select(m => m.Index).OrderByDescending(m => m);

        var enabled = true;

        var sum = 0;

        foreach (Match m in mulMatches)
        {
            var mulIndex = m.Index;

            // Get closest smallest key in dict
            var closestDoKey = doMatchIndexes.FirstOrDefault(k => k < mulIndex);
            var closestDontKey = dontMatchIndexes.FirstOrDefault(k => k < mulIndex);

            if (enabled)
            {
                if (mulIndex > closestDontKey && closestDontKey > closestDoKey)
                {
                    enabled = false;
                }
            }
            else
            {
                if (mulIndex > closestDoKey && closestDoKey > closestDontKey)
                {
                    enabled = true;
                }
            }

            if (enabled)
            {
                sum += int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value);
            }
        }

        Console.WriteLine(sum);
    }
}
