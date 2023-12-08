// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Part1("sample.txt");
Part1("input.txt");

void Part1(string filename)
{
    var lines = File.ReadAllLines(filename);

    var sum = 0;
    foreach (var line in lines)
    {
        var winningNumbers = GetWinningNumbers(line);
        var actualNumbers = GetActualNumbers(line);

        var matches = actualNumbers.Sum(number => winningNumbers.Contains(number)
            ? 1
            : 0);

        var points = matches > 2
            ? matches * 2
            : 1;

        sum += points;
    }
    
    Console.WriteLine($"Part 1 total number of points: {sum}");
}

int FindNumberOfMatches(List<int> numbers, HashSet<int> winningNumbers)
{
    return numbers.Sum(number => winningNumbers.Contains(number)
        ? 1
        : 0);
}

HashSet<int> GetWinningNumbers(string line)
{
    var colonIndex = line.AsSpan().IndexOf(':') + 1;
    var separatorIndex = line.AsSpan().IndexOf('|');
    var winningNumbers = line.AsSpan()[colonIndex..separatorIndex].ToString();

    return winningNumbers.Split(" ")
        .Where(x => x != "")
        .Select(x => int.Parse(x))
        .ToHashSet();
}

List<int> GetActualNumbers(string line)
{
    var separatorIndex = line.AsSpan().IndexOf('|') + 1;
    var numbers = line.AsSpan()[separatorIndex..].ToString();
    
    return numbers.Split(" ")
        .Where(x => x != "")
        .Select(x => int.Parse(x))
        .ToList();
}