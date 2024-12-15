namespace AdventOfCode;

public class Day11(string inputFilename) : IDay
{
    public void Part1()
    {
        var input = File.ReadAllText(inputFilename).Split(" ").Select(long.Parse);
        for (var blinkCount = 0; blinkCount < 25; blinkCount++)
        {
            input = input.SelectMany(ApplyRule).ToList();
        }
        Console.WriteLine(input.Count());
    }

    /*
       If the stone is engraved with the number 0, it is replaced by a stone engraved with the number 1.
       If the stone is engraved with a number that has an even number of digits, it is replaced by two stones. The left half of the digits are engraved on the new left stone, and the right half of the digits are engraved on the new right stone. (The new numbers don't keep extra leading zeroes: 1000 would become stones 10 and 0.)
       If none of the other rules apply, the stone is replaced by a new stone; the old stone's number multiplied by 2024 is engraved on the new stone.
     */
    private IEnumerable<long> ApplyRule(long i)
    {
        var newInput = new Stack<long>();
        if (i == 0)
        {
            newInput.Push(1);
        }
        else if (i.ToString().Length % 2 == 0)
        {
            var s = i.ToString();
            var half = s.Length / 2;
            var left = s.Substring(0, half);
            var right = s.Substring(half);
            newInput.Push(long.Parse(left));
            newInput.Push(long.Parse(right));
        }
        else
        {
            newInput.Push(i * 2024);
        }

        return newInput;
    }

    public void Part2()
    {
        var input = File.ReadAllText(inputFilename).Split(" ").Select(long.Parse).ToList();
        var newInput = input.Select(x => new InputNum()
        {
            StoneNum = x,
            Multiplier = 1
        }).ToList();

        for (var blinkCount = 0; blinkCount < 75; blinkCount++)
        {
            var groupedInput = newInput.GroupBy(i => i.StoneNum);
            newInput = new List<InputNum>();
            foreach (var i in groupedInput)
            {
                var a = ApplyRule(i.Key).ToList();
                var sum = i.ToList().Select(x => x.Multiplier).Sum();
                newInput.AddRange(a.Select(x => new InputNum()
                {
                    StoneNum = x,
                    Multiplier = sum
                }));
            }
        }
        Console.WriteLine(newInput.Select(x => x.Multiplier).Sum());
    }
}

public class InputNum
{
    public long StoneNum { get; set; }
    public long Multiplier { get; set; }
}