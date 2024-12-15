using System.Drawing;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day14(string inputFilename) : IDay
{
    private const int MaxWidth = 101;
    private const int MaxHeight = 103;
    public void Part1()
    {
        var robots = ParseInput();
        for (var i = 0; i < 100; i++)
        {
            var newRobots = new List<Robot>();
            foreach (var robot in robots)
            {
                var newPos = MoveRobot(robot);
                newRobots.Add(new Robot()
                {
                    Position = newPos,
                    VelX = robot.VelX,
                    VelY = robot.VelY
                });
            }
            robots = newRobots;
        }

        var midX = MaxWidth / 2;
        var midY = MaxHeight / 2;
        var topLeft = robots.Count(r => r.Position.X < midX && r.Position.Y < midY);
        var bottomLeft = robots.Count(r => r.Position.X < midX && r.Position.Y > midY);
        var topRight = robots.Count(r => r.Position.X > midX && r.Position.Y < midY);
        var bottomRight = robots.Count(r => r.Position.X > midX && r.Position.Y > midY);
        var safetyFactor = topLeft * topRight * bottomLeft * bottomRight;
        Console.WriteLine(safetyFactor);
    }

    private IEnumerable<Robot> ParseInput()
    {
        var input = File.ReadAllText(inputFilename);
        var regex = new Regex(@"p=(?<px>-?\d+),(?<py>-?\d+) v=(?<vx>-?\d+),(?<vy>-?\d+)");
        return regex.Matches(input).Select(m => new Robot()
        {
            Position = new Point()
            {
                X = int.Parse(m.Groups["px"].Value),
                Y = int.Parse(m.Groups["py"].Value),
            },
            VelX = int.Parse(m.Groups["vx"].Value),
            VelY = int.Parse(m.Groups["vy"].Value)
        });
    }

    private Point MoveRobot(Robot robot)
    {
        var newX = robot.Position.X + robot.VelX;
        newX = newX switch
        {
            >= MaxWidth => newX % MaxWidth,
            < 0 => MaxWidth - (Math.Abs(newX) % MaxWidth),
            _ => newX
        };

        var newY = robot.Position.Y + robot.VelY;
        newY = newY switch
        {
            >= MaxHeight => newY % MaxHeight,
            < 0 => MaxHeight - (Math.Abs(newY) % MaxHeight),
            _ => newY
        };

        return new Point()
        {
            X = newX,
            Y = newY
        };
    }

    public void Part2()
    {
        var robots = ParseInput();

        long count = 0;
        while (true)
        {
            count++;
            var newRobots = new List<Robot>();
            foreach (var robot in robots)
            {
                var newPos = MoveRobot(robot);
                newRobots.Add(new Robot()
                {
                    Position = newPos,
                    VelX = robot.VelX,
                    VelY = robot.VelY
                });
            }
            robots = newRobots;

            if (ContainsTree(robots.Select(r => r.Position)))
            {
                PrintOut(robots);
                Console.WriteLine(count);
                break;
            }

        }
    }

    private bool ContainsTree(IEnumerable<Point> robots)
    {
        var treeDepth = 0;
        robots = robots.OrderBy(r => r.Y).ThenBy(r => r.X);

        var pointsToCheck = robots.ToList();

        while (pointsToCheck.Any())
        {
            pointsToCheck = GetNextRow(robots, pointsToCheck).ToList();
            if (pointsToCheck.Any())
            {
                treeDepth++;
                // If we have 5 matching rows, just going to assume we have a tree!
                if (treeDepth == 5)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private IEnumerable<Point> GetNextRow(IEnumerable<Point> robots, IEnumerable<Point> pointsToCheck)
    {
        var lastTreePoints = new List<Point>();
        foreach (var robot in pointsToCheck)
        {
            // is there a robot directly below
            var below = robots.Where(r => r.X == robot.X && r.Y == robot.Y + 1);
            if (below.Any())
            {
                // is there a robot to the bottom left
                var bottomLeft = robots.Where(r => r.X == robot.X - 1 && r.Y == robot.Y + 1);
                if (bottomLeft.Any())
                {
                    // is there a robot to the bottom right
                    var bottomRight = robots.Where(r => r.X == robot.X + 1 && r.Y == robot.Y + 1);
                    if (bottomRight.Any())
                    {
                        lastTreePoints.AddRange(below);
                        lastTreePoints.AddRange(bottomLeft);
                        lastTreePoints.AddRange(bottomRight);
                        return lastTreePoints;
                    }
                }
            }
        }
        return new List<Point>();
    }

    private void PrintOut(IEnumerable<Robot> robots)
    {
        for (var i = 0; i < MaxHeight; i++)
        {
            for (var j = 0; j < MaxWidth; j++)
            {
                if (robots.Any(p => p.Position.X == j && p.Position.Y == i))
                {
                    Console.Write("#");
                }
                else
                {
                    Console.Write(".");
                }
            }

            Console.WriteLine("");
        }
    }
}

public class Robot
{
    public Point Position { get; set; }
    public int VelX { get; set; }
    public int VelY { get; set; }
}