// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Part1("sample.txt");
Part1("input.txt");
Part2("sample.txt");
Part2("input.txt");

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

        //var points = (uint)(1 << (matches - 1));
        var points = 0;
        if (matches >= 1)
        {
            points = 1;
            for (var i = 0; i < matches - 1; i++)
            {
                points *= 2;
            }
        }
        
        sum += points;
    }
    
    Console.WriteLine($"Part 1 total number of points: {sum}");
}

void Part2(string filename)
{
    // key: card #, value: number of cards won
    var memoizedCardsWon = new Dictionary<int, int>();
    var copiesOfWonCardIndices = new Queue<int>();
    
    var lines = File.ReadAllLines(filename);

    var totalNumberOfCardsWon = lines.Length;

    var i = 1;
    foreach (var line in lines)
    {
        var winningNumbers = GetWinningNumbers(line);
        var actualNumbers = GetActualNumbers(line);
        
        var numberOfCardsWon = actualNumbers.Sum(number => winningNumbers.Contains(number)
            ? 1
            : 0);
        memoizedCardsWon[i] = numberOfCardsWon;

        for (var j = 1; j <= numberOfCardsWon; j++)
        {
            var cardWon = i + j;
            copiesOfWonCardIndices.Enqueue(cardWon);
            totalNumberOfCardsWon += 1;
        }
        
        i++;
    }
    
    while (copiesOfWonCardIndices.Count != 0)
    {
        var cardWon = copiesOfWonCardIndices.Dequeue();
        var numberOfCardsWon = memoizedCardsWon[cardWon];
        
        for (var j = 1; j <= numberOfCardsWon; j++)
        {
            copiesOfWonCardIndices.Enqueue(cardWon+j);
            totalNumberOfCardsWon += 1;
        }
    }
    
    Console.WriteLine($"Part 2 total number of cards won: {totalNumberOfCardsWon}");
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