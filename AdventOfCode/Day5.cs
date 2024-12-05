namespace AdventOfCode;

public class Day5(string inputFilename) : IDay
{
    public void Part1()
    {
        var input = File.ReadAllLines(inputFilename);
        var rules = input.TakeWhile(i => !string.IsNullOrEmpty(i))
            .Select(i => i.Split('|').Select(int.Parse).ToArray());
        var ruleDict = new Dictionary<int, List<int>>();
        foreach (var rule in rules)
        {
            var pre = rule[0];
            var post = rule[1];
            if (ruleDict.ContainsKey(post))
            {
                ruleDict[post].Add(pre);
            }
            else
            {
                ruleDict.Add(post, [pre]);
            }
        }

        var result = input.Skip(rules.Count() + 1).Select(i => i.Split(',').Select(int.Parse).ToArray())
            .Where(u => IsUpdateInRightOrder(u, ruleDict))
            .Select(u => u[u.Length / 2])
            .Sum();
        Console.WriteLine(result);
    }

    private static bool IsUpdateInRightOrder(int[] update, Dictionary<int, List<int>> rules)
    {
        for (var i = 0; i < update.Length; i++)
        {
            if (rules.ContainsKey(update[i]))
            {
                foreach (var precedingNum in rules[update[i]])
                {
                    if (update.Contains(precedingNum) && !update.Take(i).Contains(precedingNum))
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public void Part2()
    {
        var input = File.ReadAllLines(inputFilename);
        var rules = input.TakeWhile(i => !string.IsNullOrEmpty(i))
            .Select(i => i.Split('|').Select(x => int.Parse(x)).ToArray());
        var ruleDict = new Dictionary<int, List<int>>();
        foreach (var rule in rules)
        {
            var pre = rule[0];
            var post = rule[1];
            if (ruleDict.ContainsKey(post))
            {
                ruleDict[post].Add(pre);
            }
            else
            {
                ruleDict.Add(post, [pre]);
            }
        }

        var result = input.Skip(rules.Count() + 1).Select(i => i.Split(',').Select(int.Parse).ToArray())
            .Where(u => !IsUpdateInRightOrder(u, ruleDict))
            .Select(u => RecursiveSort(u, ruleDict))
            .Select(u => u[u.Length / 2])
            .Sum();
        Console.WriteLine(result);
    }

    private static int[] RecursiveSort(int[] update, Dictionary<int, List<int>> ruleDict)
    {
        var firstHalf = update.Take(update.Length / 2);
        var secondHalf = update.Skip(update.Length / 2);
        if (firstHalf.Count() <= 2)
        {
            // Stop splitting start sorting
            var sortedFirstHalf = Sort(firstHalf.ToArray(), ruleDict);
            var sortedSecondHalf = Sort(secondHalf.ToArray(), ruleDict);
            return Sort(sortedFirstHalf.Concat(sortedSecondHalf).ToArray(), ruleDict);
        }
        return Sort(RecursiveSort(firstHalf.ToArray(), ruleDict).Concat(RecursiveSort(secondHalf.ToArray(), ruleDict)).ToArray(), ruleDict);
    }

    private static int[] Sort(int[] update, Dictionary<int, List<int>> rules)
    {
        for (var i = 0; i < update.Length; i++)
        {
            if (rules.ContainsKey(update[i]))
            {
                foreach (var precedingNum in rules[update[i]])
                {
                    if (update.Contains(precedingNum) && update.Skip(i).Contains(precedingNum))
                    {
                        // switch
                        var index = Array.IndexOf(update, precedingNum);
                        (update[i], update[index]) = (update[index], update[i]);
                        Sort(update, rules);
                    }
                }
            }
        }
        return update;
    }
}