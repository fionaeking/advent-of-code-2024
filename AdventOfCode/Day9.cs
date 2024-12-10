namespace AdventOfCode;

public class Day9(string inputFilename) : IDay
{
    public void Part1()
    {
        var input = File.ReadAllText(inputFilename)
            .Select(c => int.Parse(c.ToString())).ToArray();
        var freeSpace = new Queue<int>();
        var files = new Stack<(long, long)>();

        var id = 0;
        var currIndex = 0;
        for (var i = 0; i < input.Length; i++)
        {
            var g = Enumerable.Range(currIndex, input[i]);
            if (i % 2 == 0)
            {
                foreach (var j in g)
                {
                    files.Push((j, id));
                    currIndex++;
                }
                id++;
            }
            else
            {
                foreach (var j in g)
                {
                    freeSpace.Enqueue(j);
                    currIndex++;
                }
            }
        }

        var newFiles = new Stack<(long, long)>();
        while (freeSpace.Count > 0)
        {
            var curr = freeSpace.Dequeue();
            if (curr > files.Count + newFiles.Count)
            {
                break;
            }
            var memory = files.Pop();
            newFiles.Push((curr, memory.Item2));
        }

        var result = files.Select(f => f.Item1 * f.Item2).Sum() + newFiles.Select(f => f.Item1 * f.Item2).Sum();
        Console.WriteLine(result);
    }

    public void Part2()
    {
        var input = File.ReadAllText(inputFilename)
            .Select(c => int.Parse(c.ToString())).ToArray();
        var freeSpace = new Dictionary<long, long>(); // key is index, value is size
        var files = new Stack<DiskFile>();

        var id = 0;
        var currIndex = 0;
        for (var i = 0; i < input.Length; i++)
        {
            if (i % 2 == 0)
            {
                files.Push(new DiskFile()
                {
                    StartIndex = currIndex,
                    Size = input[i],
                    Id = id
                });
                id++;
            }
            else
            {
                if (input[i] > 0) freeSpace.Add(currIndex, input[i]);
            }
            currIndex += input[i];
        }

        long sum = 0;
        while (files.Count > 0)
        {
            var memory = files.Pop();
            var ret = freeSpace.Where(f => f.Value >= memory.Size && f.Key < memory.StartIndex);
            if (ret.Any())
            {
                var first = ret.OrderBy(k => k.Key).First();
                for (var current = 0; current < memory.Size; current++)
                {
                    sum += (first.Key + current) * memory.Id;
                }

                freeSpace.Remove(first.Key);
                var newSize = first.Value - memory.Size;
                if (newSize > 0)
                {
                    freeSpace.Add(first.Key + memory.Size, newSize);
                }

                // Also there is now free space where the file was!
                var existing =
                    freeSpace.Where(f => f.Key + f.Value == memory.StartIndex);
                if (existing.Any())
                {
                    freeSpace[existing.First().Key] += memory.Size;
                }
                else
                {
                    freeSpace.Add(memory.StartIndex, memory.Size);
                }
            }
            else
            {
                for (var current = 0; current < memory.Size; current++)
                {
                    sum += (memory.StartIndex + current) * memory.Id;
                }
            }
        }
        Console.WriteLine(sum);
    }
}

class DiskFile
{
    public long StartIndex { get; set; }
    public long Size { get; set; }
    public long Id { get; set; }
}