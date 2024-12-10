using System.Drawing;

namespace AdventOfCode;

public class Day8(string inputFilename) : IDay
{
    public void Part1()
    {
        var input = File.ReadAllLines(inputFilename).Select(x => x.ToCharArray()).ToArray();
        var points = ParseInput(input);
        var maxY = input.Length - 1;
        var maxX = input[0].Length - 1;
        var antiNodes = new List<Point>();
        foreach (var kvp in points)
        {
            foreach (var point in kvp.Value)
            {
                antiNodes.AddRange(kvp.Value
                    .SelectMany(p => CalculateAntinodes(p, point)
                        .Where(p => p.X >= 0 && p.X <= maxX && p.Y >= 0 && p.Y <= maxY)
                    ));
            }
        }
        var count = antiNodes.Distinct();
        Console.WriteLine(count.Count());
    }

    private Dictionary<char, List<Point>> ParseInput(char[][] input)
    {
        var points = new Dictionary<char, List<Point>>();
        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input.Length; j++)
            {
                if (input[i][j] != '.')
                {
                    var point = new Point()
                    {
                        X = j,
                        Y = i,
                    };
                    if (!points.ContainsKey(input[i][j]))
                    {
                        points.Add(input[i][j], [point]);
                    }
                    else
                    {
                        points[input[i][j]].Add(point);
                    }
                }
            }
        }
        return points;
    }

    private List<Point> CalculateAntinodes(Point p1, Point p2)
    {
        var diffY = Math.Abs(p2.Y - p1.Y);
        var diffX = Math.Abs(p2.X - p1.X);
        if (diffY == 0 && diffX == 0) return [];
        return
        [
            new Point()
            {
                X = (p1.X < p2.X) ? p1.X - diffX : p1.X + diffX,
                Y = (p1.Y < p2.Y) ? p1.Y - diffY : p1.Y + diffY
            },
            new Point()
            {
                X = (p1.X < p2.X) ? p2.X + diffX : p2.X - diffX,
                Y = (p1.Y < p2.Y) ? p2.Y + diffY : p2.Y - diffY
            }
        ];
    }

    private List<Point> CalculateAntinodesPart2(Point p1, Point p2, int maxX, int maxY)
    {
        var diffY = Math.Abs(p2.Y - p1.Y);
        var diffX = Math.Abs(p2.X - p1.X);
        if (diffY == 0 && diffX == 0) return [];
        var newPoints = new List<Point>() { p1, p2 };
        var loopMultiplier = 1;
        while (true)
        {
            var newX = (p1.X < p2.X) ? p1.X - (diffX * loopMultiplier) : p1.X + (diffX * loopMultiplier);
            var newY = (p1.Y < p2.Y) ? p1.Y - (diffY * loopMultiplier) : p1.Y + (diffY * loopMultiplier);
            if (newX >= 0 && newX <= maxX && newY >= 0 && newY <= maxY)
            {
                newPoints.Add(new Point() { X = newX, Y = newY });
            }
            else
            {
                break;
            }
            loopMultiplier++;
        }

        loopMultiplier = 1;
        while (true)
        {
            var newX = (p1.X < p2.X) ? p2.X + (diffX * loopMultiplier) : p2.X - (diffX * loopMultiplier);
            var newY = (p1.Y < p2.Y) ? p2.Y + (diffY * loopMultiplier) : p2.Y - (diffY * loopMultiplier);
            if (newX >= 0 && newX <= maxX && newY >= 0 && newY <= maxY)
            {
                newPoints.Add(new Point() { X = newX, Y = newY });
            }
            else
            {
                break;
            }
            loopMultiplier++;
        }
        return newPoints;
    }

    public void Part2()
    {
        var input = File.ReadAllLines(inputFilename).Select(x => x.ToCharArray()).ToArray();
        var points = ParseInput(input);
        var maxY = input.Length - 1;
        var maxX = input[0].Length - 1;
        var antiNodes = new List<Point>();
        foreach (var kvp in points)
        {
            foreach (var point in kvp.Value)
            {
                antiNodes.AddRange(kvp.Value
                    .SelectMany(p => CalculateAntinodesPart2(p, point, maxX, maxY)
                    ));
            }
        }
        var count = antiNodes.Distinct();
        Console.WriteLine(count.Count());
    }


}