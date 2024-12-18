using System.Drawing;

namespace AdventOfCode;

public class Day15(string inputFilename) : IDay
{
    public void Part1()
    {
        var input = File.ReadAllLines(inputFilename);
        var splitIndex = Array.IndexOf(input, string.Empty);
        var warehouseInput = input.Take(splitIndex).ToArray();
        var instructions = string.Join("", input.Skip(splitIndex + 1));
        var warehouseMap = new Dictionary<Point, char>();
        var robotPosition = new Point();
        for (var i = 0; i < warehouseInput.Length; i++)
        {
            for (var j = 0; j < warehouseInput[i].Length; j++)
            {
                switch (input[i][j])
                {
                    case '@':
                        robotPosition = new Point(j, i);
                        break;
                    case 'O':
                    case '#':
                        warehouseMap.Add(new Point(j, i), input[i][j]);
                        break;
                }
            }
        }
        warehouseMap = MoveRobot(robotPosition, warehouseMap, instructions);
        var sum = warehouseMap.Where(kvp => kvp.Value == 'O').Select(x => 100 * x.Key.Y + x.Key.X).Sum();
        Console.WriteLine(sum);
    }

    private Dictionary<Point, char> MoveRobot(Point robotPosition, Dictionary<Point, char> warehouseMap, string instructions)
    {
        var currentPos = robotPosition;
        foreach (var instruction in instructions)
        {
            var g = CanItMove(currentPos, warehouseMap, instruction);
            if (g is null)
            {
                continue;
            }
            var newRobotPos = GetNextPosition(instruction, currentPos);
            if (newRobotPos != g)
            {
                // boxes have been moved
                var boxChar = warehouseMap[newRobotPos];
                warehouseMap.Remove(newRobotPos);
                warehouseMap.Add((Point)g, boxChar);
            }
            currentPos = newRobotPos;
        }

        return warehouseMap;
    }

    private Point? CanItMove(Point currPos, Dictionary<Point, char> warehouseMap, char instruction)
    {
        while (true)
        {
            var newPos = GetNextPosition(instruction, currPos);
            if (!warehouseMap.TryGetValue(newPos, out var value)) return newPos;
            if (value == '#') return null;
            currPos = newPos;
        }
    }

    private Point GetNextPosition(char instruction, Point currentPos)
    {
        return instruction switch
        {
            '^' => currentPos with { Y = currentPos.Y - 1 },
            'v' => currentPos with { Y = currentPos.Y + 1 },
            '>' => currentPos with { X = currentPos.X + 1 },
            '<' => currentPos with { X = currentPos.X - 1 },
            _ => throw new InvalidOperationException()
        };
    }

    public void Part2()
    {
        var input = File.ReadAllLines(inputFilename);
        var splitIndex = Array.IndexOf(input, string.Empty);
        var warehouseInput = input.Take(splitIndex).ToArray();
        var instructions = string.Join("", input.Skip(splitIndex + 1));
        var warehouseMap = new Dictionary<Point, char>();
        var robotPosition = new Point();
        for (var i = 0; i < warehouseInput.Length; i++)
        {
            for (var j = 0; j < warehouseInput[i].Length; j = j + 1)
            {
                switch (input[i][j])
                {
                    case '@':
                        robotPosition = new Point(2 * j, i);
                        break;
                    case 'O':
                        warehouseMap.Add(new Point(2 * j, i), '[');
                        warehouseMap.Add(new Point(2 * j + 1, i), ']');
                        break;
                    case '#':
                        warehouseMap.Add(new Point(2 * j, i), input[i][j]);
                        warehouseMap.Add(new Point(2 * j + 1, i), input[i][j]);
                        break;
                }
            }
        }
        warehouseMap = MoveRobotTwo(robotPosition, warehouseMap, instructions);
        var sum = warehouseMap.Where(kvp => kvp.Value == '[').Select(x => 100 * x.Key.Y + x.Key.X).Sum();
        Console.WriteLine(sum);
    }

    private Dictionary<Point, char> MoveRobotTwo(Point robotPosition, Dictionary<Point, char> warehouseMap,
        string instructions)
    {
        var currentPos = robotPosition;
        foreach (var instruction in instructions)
        {
            // account for immutable/mutable mess!
            var warehouseMapClone = new Dictionary<Point, char>(warehouseMap);
            var g = CanItMoveTwo(currentPos, warehouseMap, instruction);
            if (g is null)
            {
                warehouseMap = warehouseMapClone;
            }
            else
            {
                warehouseMap = g;
                currentPos = GetNextPosition(instruction, currentPos);
            }
        }
        return warehouseMap;
    }

    private Dictionary<Point, char>? CanItMoveTwo(Point currPos, Dictionary<Point, char> warehouseMap, char instruction)
    {
        var newPos = GetNextPosition(instruction, currPos);
        if (warehouseMap.ContainsKey(newPos))
        {
            switch (warehouseMap[newPos])
            {
                case '#':
                    // wall - don't move
                    return null;
                case '[':
                    {
                        if (instruction == '>')
                        {
                            var rightCheck = GetNextPosition('>', newPos);

                            var g = CanItMoveTwo(rightCheck, warehouseMap, instruction);
                            if (g is null)
                            {
                                return null;
                            }

                            warehouseMap = g;

                            var f = CanItMoveTwo(newPos, warehouseMap, instruction);
                            if (f is null)
                            {
                                return null;
                            }

                            warehouseMap = f;
                        }
                        else
                        {
                            // box in way - check if can move
                            var f = CanItMoveTwo(newPos, warehouseMap, instruction);
                            if (f is null)
                            {
                                return null;
                            }
                            warehouseMap = f;

                            var rightCheck = GetNextPosition('>', newPos);
                            var g = CanItMoveTwo(rightCheck, warehouseMap, instruction);
                            if (g is null)
                            {
                                return null;
                            }
                            warehouseMap = g;
                        }

                        if (!warehouseMap.TryGetValue(currPos, out var chL))
                        {
                            return warehouseMap;
                        }

                        warehouseMap.Remove(currPos);
                        warehouseMap[newPos] = chL;
                        return warehouseMap;
                    }
                case ']':
                    {
                        if (instruction == '<')
                        {
                            // box in way - check if can move
                            var leftCheck = GetNextPosition('<', newPos);
                            var g = CanItMoveTwo(leftCheck, warehouseMap, instruction);
                            if (g is null)
                            {
                                return null;
                            }
                            warehouseMap = g;
                            var f = CanItMoveTwo(newPos, warehouseMap, instruction);
                            if (f is null)
                            {
                                return null;
                            }
                            warehouseMap = f;
                        }
                        else
                        {
                            // box in way - check if can move
                            var leftCheck = GetNextPosition('<', newPos);
                            var f = CanItMoveTwo(newPos, warehouseMap, instruction);
                            if (f is null)
                            {
                                return null;
                            }

                            warehouseMap = f;
                            var g = CanItMoveTwo(leftCheck, warehouseMap, instruction);
                            if (g is null)
                            {
                                return null;
                            }
                            warehouseMap = g;
                        }

                        if (!warehouseMap.TryGetValue(currPos, out var chR))
                        {
                            return warehouseMap;
                        }

                        warehouseMap.Remove(currPos);
                        warehouseMap[newPos] = chR;
                        return warehouseMap;
                    }
            }
        }

        // i.e. if not robot
        if (!warehouseMap.TryGetValue(currPos, out var ch))
        {
            return warehouseMap;
        }

        warehouseMap.Remove(currPos);
        warehouseMap[newPos] = ch;

        return warehouseMap;
    }
}
