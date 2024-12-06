﻿using System.Drawing;

namespace AdventOfCode;

public class Day6(string inputFilename) : IDay
{
    public void Part1()
    {
        var input = File.ReadAllLines(inputFilename).Select(x => x.ToCharArray()).ToArray();
        var currPoint = GetStartPoint(input);
        var visitedPointsDirections = GetVisitedPoints(input, currPoint);
        Console.WriteLine($"Part 1: {visitedPointsDirections.Select(x => x.Item1).Distinct().Count()}");
    }

    private List<(Point, Direction)> GetVisitedPoints(char[][] input, Point currPoint)
    {
        var visitedPointsDirections = new List<(Point, Direction)>() { (currPoint, Direction.Up) };
        var currDirection = Direction.Up;
        while (true)
        {
            switch (currDirection)
            {
                case Direction.Up:
                    var nextYup = currPoint.Y - 1;
                    if (nextYup < 0)
                    {
                        return visitedPointsDirections;
                    }

                    if (input[nextYup][currPoint.X] == '#')
                    {
                        currDirection = Direction.Right;
                    }
                    else
                    {
                        currPoint = currPoint with { Y = nextYup };
                    }

                    break;
                case Direction.Down:
                    var nextYdown = currPoint.Y + 1;
                    if (nextYdown > input.Length - 1)
                    {
                        return visitedPointsDirections;
                    }

                    if (input[nextYdown][currPoint.X] == '#')
                    {
                        currDirection = Direction.Left;
                    }
                    else
                    {
                        currPoint = currPoint with { Y = nextYdown };
                    }

                    break;
                case Direction.Right:
                    var nextXright = currPoint.X + 1;
                    if (nextXright > input[0].Length - 1)
                    {
                        return visitedPointsDirections;
                    }

                    if (input[currPoint.Y][nextXright] == '#')
                    {
                        currDirection = Direction.Down;
                    }
                    else
                    {
                        currPoint = currPoint with { X = nextXright };
                    }
                    break;
                case Direction.Left:
                    var nextXleft = currPoint.X - 1;
                    if (nextXleft < 0)
                    {
                        return visitedPointsDirections;
                    }

                    if (input[currPoint.Y][nextXleft] == '#')
                    {
                        currDirection = Direction.Up;
                    }
                    else
                    {
                        currPoint = currPoint with { X = nextXleft };
                    }
                    break;
            }

            if (visitedPointsDirections.Any(d =>
                    d.Item1.X == currPoint.X && d.Item1.Y == currPoint.Y && d.Item2 == currDirection))
            {
                throw new InfiniteLoopException();
            }
            visitedPointsDirections.Add((currPoint, currDirection));
        }
    }

    private Point GetStartPoint(char[][] input)
    {
        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[0].Length; j++)
            {
                var ch = input[i][j];
                if (ch == '^')
                {
                    return new Point()
                    {
                        X = j,
                        Y = i
                    };
                }
            }
        }
        throw new Exception();
    }

    public void Part2()
    {
        // TODO extremely slow!
        var input = File.ReadAllLines(inputFilename).Select(x => x.ToCharArray()).ToArray();
        var currPoint = GetStartPoint(input);

        var visitedPointsDirections = GetVisitedPoints(input, currPoint);

        // For each point visited, we will try to replace it with a wall and check if it is still an infinite loop
        var allPossibleInputs = new List<char[][]>() { input };

        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[0].Length; j++)
            {
                if (visitedPointsDirections.Any(x => x.Item1.X == j && x.Item1.Y == i))
                {
                    var ch = input[i][j];
                    if (ch != '^')
                    {
                        // Read in again to get deep clone of input array
                        var copyArray = File.ReadAllLines(inputFilename).Select(x => x.ToCharArray()).ToArray();
                        copyArray[i][j] = '#';
                        allPossibleInputs.Add(copyArray);
                    }
                }
            }
        }

        var count = allPossibleInputs.Count(i => IsInfiniteLoop(i, currPoint));
        Console.WriteLine($"Part 2: {count}");
    }

    private bool IsInfiniteLoop(char[][] i, Point currPoint)
    {
        try
        {
            GetVisitedPoints(i, currPoint);
            return false;
        }
        catch (InfiniteLoopException e)
        {
            return true;
        }
    }

}

public enum Direction
{
    Up, Down, Left, Right
}

// Create new exception type
public class InfiniteLoopException : Exception
{
    public InfiniteLoopException()
    {
    }
    public InfiniteLoopException(string message)
        : base(message)
    {
    }
    public InfiniteLoopException(string message, Exception inner)
        : base(message, inner)
    {
    }
}