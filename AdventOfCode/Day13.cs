using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day13(string inputFilename) : IDay
{
    public void Part1()
    {
        Console.WriteLine(GetTokenCost(false));
    }

    private IEnumerable<ClawMachine> ParseInput(bool partTwo = false)
    {
        var input = File.ReadAllText(inputFilename);
        var regex = new Regex(@"Button A: X\+(?<buttonAX>\d+), Y\+(?<buttonAY>\d+)\r\nButton B: X\+(?<buttonBX>\d+), Y\+(?<buttonBY>\d+)\r\nPrize: X=(?<prizeX>\d+), Y=(?<prizeY>\d+)");
        return regex.Matches(input).Select(m => new ClawMachine()
        {
            ButtonA = new Button()
            {
                XMovement = int.Parse(m.Groups["buttonAX"].Value),
                YMovement = int.Parse(m.Groups["buttonAY"].Value)
            },
            ButtonB = new Button()
            {
                XMovement = int.Parse(m.Groups["buttonBX"].Value),
                YMovement = int.Parse(m.Groups["buttonBY"].Value)
            },
            Prize = new Prize()
            {
                X = partTwo ? long.Parse(m.Groups["prizeX"].Value) + 10000000000000 : long.Parse(m.Groups["prizeX"].Value),
                Y = partTwo ? long.Parse(m.Groups["prizeY"].Value) + 10000000000000 : long.Parse(m.Groups["prizeY"].Value),
            }
        });
    }

    public void Part2()
    {
        Console.WriteLine(GetTokenCost(true));
    }

    private long GetTokenCost(bool partTwo = false)
    {
        var clawMachines = ParseInput(partTwo);
        long tokenCost = 0;
        foreach (var clawMachine in clawMachines)
        {
            double lcmA = FindLCM(clawMachine.ButtonA.XMovement, clawMachine.ButtonA.YMovement);
            var xMult = lcmA / clawMachine.ButtonA.XMovement;
            var yMult = lcmA / clawMachine.ButtonA.YMovement;
            var xPrize = clawMachine.Prize.X * xMult;
            var yPrize = clawMachine.Prize.Y * yMult;
            var xb = clawMachine.ButtonB.XMovement * xMult;
            var yb = clawMachine.ButtonB.YMovement * yMult;
            var b = (yPrize - xPrize) / (yb - xb);
            var a = (clawMachine.Prize.X - clawMachine.ButtonB.XMovement * b) / clawMachine.ButtonA.XMovement;
            if (a % 1 != 0 || b % 1 != 0)
            {
                continue;
            }

            if (!partTwo && (b > 100 || a > 100))
            {
                continue;
            }
            tokenCost += ((3 * (long)a) + (long)b);
        }
        return tokenCost;
    }

    private static int FindGCD(int num1, int num2)
    {
        while (num2 != 0)
        {
            var temp = num2;
            num2 = num1 % num2;
            num1 = temp;
        }
        return num1;
    }

    private static int FindLCM(int num1, int num2)
    {
        return (num1 * num2) / FindGCD(num1, num2);
    }
}

public class Button
{
    public int XMovement { get; set; }
    public int YMovement { get; set; }
}

public class Prize
{
    public long X { get; set; }
    public long Y { get; set; }
}

public class ClawMachine
{
    public Button ButtonA { get; set; }
    public Button ButtonB { get; set; }
    public Prize Prize { get; set; }
}