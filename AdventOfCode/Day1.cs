namespace AdventOfCode;
public class Day1(string inputFilename) : IDay
{
    public void Part1()
    {
        var input = File.ReadAllLines(inputFilename)
            .Select(line => line.Split(" ").Select(int.Parse)).ToList();
        var firstList = input.Select(item => item.First()).Order();
        var secondList = input.Select(item => item.Last()).Order();
        var result = firstList.Zip(secondList, (a, b) => Math.Abs(a - b)).Sum();
        Console.WriteLine(result);
    }

    public void Part2()
    {
        var input = File.ReadAllLines(inputFilename)
            .Select(line => line.Split(" ").Select(int.Parse)).ToList();
        var firstList = input.Select(item => item.First());
        var secondList = input.Select(item => item.Last());

        var result = firstList
            .Select(firstItem => firstItem * secondList.Count(secondItem => secondItem == firstItem))
            .Sum();
        Console.WriteLine(result);
    }
}
