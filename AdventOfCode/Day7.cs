namespace AdventOfCode;

public class Day7(string inputFilename) : IDay
{
    public void Part1()
    {
        var result = ParseInput(inputFilename).Where(i => IsPossibleEquation(i)).Sum(i => i.TestValue);
        Console.WriteLine(result);
    }

    public void Part2()
    {
        var result = ParseInput(inputFilename).Where(i => IsPossibleEquation(i, true)).Sum(i => i.TestValue);
        Console.WriteLine(result);
    }

    private static bool IsPossibleEquation(Equation equation, bool includeConcatenation = false)
    {
        var targetValue = equation.TestValue;
        var possibleResults = new List<long>() { equation.Inputs[0] };
        foreach (var input in equation.Inputs.Skip(1))
        {
            var newValues = possibleResults.Select(p => p + input).ToList();
            newValues.AddRange(possibleResults.Select(p => p * input));
            if (includeConcatenation)
            {
                newValues.AddRange(possibleResults.Select(p => long.Parse(p.ToString() + input)));
            }
            possibleResults = newValues.Where(p => p <= targetValue).ToList();
        }
        return possibleResults.Contains(targetValue);
    }

    private static IEnumerable<Equation> ParseInput(string inputFilename)
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