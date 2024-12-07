namespace AdventOfCode;

public class Day7(string inputFilename) : IDay
{
    public void Part1()
    {
        var result = ParseInput().Where(equation => equation.Inputs.Skip(1)
                .Aggregate(new List<long> { equation.Inputs[0] }, (current, input) => current.SelectMany(p => new List<long>() { p + input, p * input })
                    .Where(p => p <= equation.TestValue).ToList()).Contains(equation.TestValue))
            .Sum(i => i.TestValue);
        Console.WriteLine(result);
    }

    public void Part2()
    {
        var result = ParseInput().Where(equation => equation.Inputs.Skip(1)
                .Aggregate(new List<long> { equation.Inputs[0] }, (current, input) => current.SelectMany(p => new List<long>() { p + input, p * input, long.Parse(p.ToString() + input) })
                    .Where(p => p <= equation.TestValue).ToList()).Contains(equation.TestValue))
            .Sum(i => i.TestValue);
        Console.WriteLine(result);
    }

    private IEnumerable<Equation> ParseInput()
    {
        return File.ReadAllLines(inputFilename)
            .Select(i => i.Split(":"))
                .Select(i => new Equation()
                {
                    TestValue = long.Parse(i[0]),
                    Inputs = i[1].Split(" ").Where(s => !string.IsNullOrEmpty(s)).Select(long.Parse).ToList()
                });
    }
}

public class Equation
{
    public long TestValue { get; set; }
    public List<long> Inputs { get; set; }
}