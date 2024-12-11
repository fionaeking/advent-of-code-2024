
using System.Drawing;

namespace AdventOfCode;

public class Day10(string inputFilename) : IDay
{
    public void Part1()
    {
        var input = File.ReadAllLines(inputFilename)
            .Select(b => b.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
        var score = 0;
        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] == 0)
                {
                    // Check if hiking trail
                    var a = GetAllHikingTrails(i, j, input).Count();
                    score += a;
                }
            }
        }
        Console.WriteLine(score);
    }

    private IEnumerable<Point> GetAllHikingTrails(int i, int j, int[][] input)
    {
        var currVal = input[i][j];
        if (currVal == 9)
        {
            return new List<Point> { new Point(j, i) };
        }

        var squaresToCheck = new List<Point>();
        if (i - 1 >= 0)
        {
            squaresToCheck.Add(new Point(j, i - 1));
        }
        if (i + 1 < input.Length)
        {
            squaresToCheck.Add(new Point(j, i + 1));
        }
        if (j - 1 >= 0)
        {
            squaresToCheck.Add(new Point(j - 1, i));
        }
        if (j + 1 < input[i].Length)
        {
            squaresToCheck.Add(new Point(j + 1, i));
        }
        return squaresToCheck.Where(x => input[x.Y][x.X] == currVal + 1).SelectMany(x => GetAllHikingTrails(x.Y, x.X, input)).Distinct();
    }

    public void Part2()
    {
        throw new NotImplementedException();
    }
}