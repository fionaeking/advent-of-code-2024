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
                if (input[i][j] == '@')
                {
                    robotPosition = new Point(j, i);
                }
                else if (input[i][j] == 'O' || input[i][j] == '#')
                {
                    warehouseMap.Add(new Point(j, i), input[i][j]);
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
        var newPos = GetNextPosition(instruction, currPos);
        if (warehouseMap.ContainsKey(newPos))
        {
            if (warehouseMap[newPos] == '#')
            {
                // wall - don't move
                return null;
            }
            // box in way - check if can move
            return CanItMove(newPos, warehouseMap, instruction);
        }
        return newPos;
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
                if (input[i][j] == '@')
                {
                    robotPosition = new Point(2 * j, i);
                }
                else if (input[i][j] == 'O')
                {
                    warehouseMap.Add(new Point(2 * j, i), '[');
                    warehouseMap.Add(new Point(2 * j + 1, i), ']');
                }
                else if (input[i][j] == '#')
                {
                    warehouseMap.Add(new Point(2 * j, i), input[i][j]);
                    warehouseMap.Add(new Point(2 * j + 1, i), input[i][j]);
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
            // deep clone warehouseMap
            var newWarehouseMap = new Dictionary<Point, char>(warehouseMap);
            var g = CanItMoveTwo(currentPos, warehouseMap, instruction);
            if (g is null)
            {
                warehouseMap = newWarehouseMap;
            }
            else
            {
                warehouseMap = g;
                currentPos = GetNextPosition(instruction, currentPos);
            }

            ////print warehouse map
            //for (var i = 0; i <= warehouseMap.Keys.MaxBy(f => f.Y).Y; i++)
            //{
            //    for (var j = 0; j <= warehouseMap.Keys.MaxBy(f => f.X).X; j++)
            //    {
            //        if (warehouseMap.ContainsKey(new Point(j, i)))
            //        {
            //            Console.Write(warehouseMap[new Point(j, i)]);
            //        }
            //        else if (j == currentPos.X && i == currentPos.Y)
            //        {
            //            Console.Write("@");
            //        }
            //        else
            //        {
            //            Console.Write('.');
            //        }
            //    }
            //
            //    Console.WriteLine();
            //}


        }
        return warehouseMap;
    }

    private Dictionary<Point, char>? CanItMoveTwo(Point currPos, Dictionary<Point, char> warehouseMap, char instruction)
    {
        //if (instruction == '^')
        // {
        //     Console.WriteLine("------");
        // }
        var newPos = GetNextPosition(instruction, currPos);
        if (warehouseMap.ContainsKey(newPos))
        {
            if (warehouseMap[newPos] == '#')
            {
                // wall - don't move
                return null;
            }
            if (warehouseMap[newPos] == '[')
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

                if (!warehouseMap.ContainsKey(currPos))
                {
                    return warehouseMap;
                }
                var chh = warehouseMap[currPos];
                warehouseMap.Remove(currPos);
                warehouseMap[newPos] = chh;
                return warehouseMap;
            }
            if (warehouseMap[newPos] == ']')
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

                if (!warehouseMap.ContainsKey(currPos))
                {
                    return warehouseMap;
                }
                var chhhh = warehouseMap[currPos];
                warehouseMap.Remove(currPos);
                warehouseMap[newPos] = chhhh;
                return warehouseMap;
            }
            //var l = CanItMoveTwo(newPos, warehouseMap, instruction);
            //l.Add(newPos);
            //return l;
        }

        // i.e. if not robot
        if (!warehouseMap.ContainsKey(currPos))
        {
            return warehouseMap;
        }
        var ch = warehouseMap[currPos];
        warehouseMap.Remove(currPos);
        warehouseMap[newPos] = ch;

        return warehouseMap;
    }
}
