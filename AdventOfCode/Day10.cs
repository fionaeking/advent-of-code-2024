
using System.Drawing;

namespace AdventOfCode;

public class Day10(string inputFilename) : IDay
{
    public void Part1()
    {
        var input = ParseInput();
        var score = input.SelectMany((row, i) => row.Select((val, j) => new { val, i, j }))
            .Where(x => x.val == 0)
            .Sum(x => GetAllHikingTrails(x.i, x.j, input).Distinct().Count());
        Console.WriteLine(score);
    }

    private int[][] ParseInput()
    {
        return File.ReadAllLines(inputFilename)
            .Select(b => b.Select(c => int.Parse(c.ToString())).ToArray()).ToArray(); ;
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
        return squaresToCheck.Where(x => input[x.Y][x.X] == currVal + 1).SelectMany(x => GetAllHikingTrails(x.Y, x.X, input));
    }

    public void Part2()
    {
        var input = ParseInput();
        var score = input.SelectMany((row, i) => row.Select((val, j) => new { val, i, j }))
            .Where(x => x.val == 0)
            .Sum(x => GetAllHikingTrails(x.i, x.j, input).Count());
        Console.WriteLine(score);
    }
}