namespace AdventOfCode;

public class Day4(string inputFilename) : IDay
{
    public void Part1()
    {
        var input = File.ReadAllLines(inputFilename);
        var count = 0;
        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[0].Length; j++)
            {
                if (input[i][j] == 'X')
                {
                    if (j + 3 < input[0].Length)
                    {
                        if (input[i][j + 1] == 'M' && input[i][j + 2] == 'A' && input[i][j + 3] == 'S')
                        {
                            count++;
                        }
                    }

                    if (j - 3 >= 0)
                    {
                        if (input[i][j - 1] == 'M' && input[i][j - 2] == 'A' && input[i][j - 3] == 'S')
                        {
                            count++;
                        }
                    }

                    if (i + 3 < input.Length)
                    {
                        if (input[i + 1][j] == 'M' && input[i + 2][j] == 'A' && input[i + 3][j] == 'S')
                        {
                            count++;
                        }
                    }

                    if (i - 3 >= 0)
                    {
                        if (input[i - 1][j] == 'M' && input[i - 2][j] == 'A' && input[i - 3][j] == 'S')
                        {
                            count++;
                        }
                    }

                    if (i + 3 < input.Length && j + 3 < input[0].Length)
                    {
                        if (input[i + 1][j + 1] == 'M' && input[i + 2][j + 2] == 'A' && input[i + 3][j + 3] == 'S')
                        {
                            count++;
                        }
                    }

                    if (i - 3 >= 0 && j - 3 >= 0)
                    {
                        if (input[i - 1][j - 1] == 'M' && input[i - 2][j - 2] == 'A' && input[i - 3][j - 3] == 'S')
                        {
                            count++;
                        }
                    }

                    if (i + 3 < input.Length && j - 3 >= 0)
                    {
                        if (input[i + 1][j - 1] == 'M' && input[i + 2][j - 2] == 'A' && input[i + 3][j - 3] == 'S')
                        {
                            count++;
                        }
                    }

                    if (i - 3 >= 0 && j + 3 < input[0].Length)
                    {
                        if (input[i - 1][j + 1] == 'M' && input[i - 2][j + 2] == 'A' && input[i - 3][j + 3] == 'S')
                        {
                            count++;
                        }
                    }
                }
            }
        }
        Console.WriteLine(count);
    }

    public void Part2()
    {
        var input = File.ReadAllLines(inputFilename);
        var count = 0;
        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[0].Length; j++)
            {
                if (input[i][j] == 'A' && i - 1 >= 0 && i + 1 < input.Length && j - 1 >= 0 && j + 1 < input[0].Length)
                {
                    var topLeft = input[i - 1][j - 1];
                    var topRight = input[i - 1][j + 1];
                    var bottomLeft = input[i + 1][j - 1];
                    var bottomRight = input[i + 1][j + 1];
                    if ((topLeft == 'M' && topRight == 'S' && bottomLeft == 'M' && bottomRight == 'S')
                        || (topLeft == 'S' && topRight == 'M' && bottomLeft == 'S' && bottomRight == 'M')
                        || (topLeft == 'M' && topRight == 'M' && bottomLeft == 'S' && bottomRight == 'S')
                        || (topLeft == 'S' && topRight == 'S' && bottomLeft == 'M' && bottomRight == 'M'))
                    {
                        count++;
                    }
                }
            }
        }
        Console.WriteLine(count);
    }
}
