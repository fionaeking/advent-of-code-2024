using System.Drawing;

namespace AdventOfCode;

public class Day12(string inputFilename) : IDay
{
    public void Part1()
    {
        var input = File.ReadAllLines(inputFilename).Select(line => line.Select(ch => ch).ToArray()).ToArray();
        var regions = CalculateRegions(input);
        var totalPrice = 0;
        // Now we have regions, work out perimeters
        foreach (var region in regions)
        {
            foreach (var regionValue in region.Value)
            {
                var perimeter = CalculatePerimeter(regionValue.Value);
                var area = regionValue.Value.Count;
                totalPrice += perimeter * area;
            }
        }
        Console.WriteLine(totalPrice);
    }

    private Dictionary<char, Dictionary<Guid, List<Point>>> CalculateRegions(char[][] input)
    {
        var regions = new Dictionary<char, Dictionary<Guid, List<Point>>>();
        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[i].Length; j++)
            {
                var currValue = input[i][j];
                if (regions.ContainsKey(currValue))
                {
                    var matches = regions[currValue].Where(region => region.Value.Any(p => Math.Abs(p.Y - i) + Math.Abs(p.X - j) == 1));
                    if (matches.Any())
                    {
                        // Merge if multiple matches
                        var newList = matches.SelectMany(h => h.Value).ToList();

                        foreach (var key in matches.Select(l => l.Key))
                        {
                            regions[currValue].Remove(key);
                        }
                        newList.Add(new Point()
                        {
                            X = j,
                            Y = i
                        });
                        regions[currValue].Add(Guid.NewGuid(), newList);
                    }
                    else
                    {
                        regions[currValue].Add(Guid.NewGuid(), [
                            new Point()
                            {
                                X = j,
                                Y = i
                            }
                        ]);
                    }
                }
                else
                {
                    regions.Add(currValue, new Dictionary<Guid, List<Point>>()
                    {
                        {
                            Guid.NewGuid(), [
                                new Point
                                {
                                    X = j,
                                    Y = i
                                }
                            ]
                        }
                    });
                }
            }
        }

        return regions;
    }

    private int CalculatePerimeter(List<Point> points)
    {
        var perimeter = 0;
        foreach (var point in points)
        {
            // check L, R, U, D
            if (!points.Any(p => p.X == point.X - 1 && p.Y == point.Y))
            {
                perimeter++;
            }
            if (!points.Any(p => p.X == point.X + 1 && p.Y == point.Y))
            {
                perimeter++;
            }
            if (!points.Any(p => p.X == point.X && p.Y - 1 == point.Y))
            {
                perimeter++;
            }
            if (!points.Any(p => p.X == point.X && p.Y + 1 == point.Y))
            {
                perimeter++;
            }
        }
        return perimeter;
    }

    public void Part2()
    {
        var input = File.ReadAllLines(inputFilename).Select(line => line.Select(ch => ch).ToArray()).ToArray();
        var regions = CalculateRegions(input);
        var totalPrice = 0;
        // Now we have regions, work out perimeters
        foreach (var region in regions)
        {
            foreach (var regionValue in region.Value)
            {
                var sides = CalculateAllSides(regionValue.Value);
                var area = regionValue.Value.Count;
                totalPrice += sides * area;
            }
        }
        Console.WriteLine(totalPrice);
    }

    private int CalculateAllSides(List<Point> points)
    {
        var upPoints = new List<Point>();
        var downPoints = new List<Point>();
        var leftPoints = new List<Point>();
        var rightPoints = new List<Point>();
        foreach (var point in points)
        {
            // check L, R, U, D
            if (!points.Any(p => p.X == point.X - 1 && p.Y == point.Y))
            {
                leftPoints.Add(point);
            }
            if (!points.Any(p => p.X == point.X + 1 && p.Y == point.Y))
            {
                rightPoints.Add(point);
            }
            if (!points.Any(p => p.X == point.X && p.Y - 1 == point.Y))
            {
                upPoints.Add(point);
            }
            if (!points.Any(p => p.X == point.X && p.Y + 1 == point.Y))
            {
                downPoints.Add(point);
            }
        }
        return CalculateSides(downPoints) + CalculateSides(upPoints) + CalculateSides(rightPoints) + CalculateSides(leftPoints);
    }

    private int CalculateSides(List<Point> input)
    {
        var regions = new Dictionary<Guid, List<Point>>();
        foreach (var point in input)
        {
            var matches = regions.Where(region => region.Value.Any(p => Math.Abs(p.Y - point.Y) + Math.Abs(p.X - point.X) == 1));
            if (matches.Any())
            {
                // Merge if multiple matches
                var newList = matches.SelectMany(h => h.Value).ToList();

                foreach (var key in matches.Select(l => l.Key))
                {
                    regions.Remove(key);
                }
                newList.Add(point);
                regions.Add(Guid.NewGuid(), newList);
            }
            else
            {
                regions.Add(Guid.NewGuid(), [point]);
            }
        }
        return regions.Count;
    }
}