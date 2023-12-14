// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Part1("sample.txt");
Part1("input.txt");

Part2("sample.txt");
Part2("input.txt");

void Part1(string filename)
{
    var raceInputs = ParseRaceInputs(filename)
        .ToList();

    var raceWinningWays = new List<ulong>();
    foreach (var input in raceInputs)
    {
        var numOfWinningWays = CalculateNumberOfWinningWays(input);
        raceWinningWays.Add(numOfWinningWays);
    }

    ulong marginOfError = 1;
    foreach (var numOfWinningWays in raceWinningWays)
    {
        marginOfError *= numOfWinningWays;
    }
    
    Console.WriteLine($"Part 1 - Margin of Error: {marginOfError}");
}

void Part2(string filename)
{
    var raceInputs = ParsePart2RaceInputs(filename);
    var numOfWinningWays = CalculateNumberOfWinningWays(raceInputs);
    
    Console.WriteLine($"Part 2 - Number of ways to win: {numOfWinningWays}");
}

ulong CalculateNumberOfWinningWays((ulong time, ulong distance) raceInput)
{
    ulong numOfWinningWays = 0; 
    for (ulong holdTimeAndSpeed = 0; holdTimeAndSpeed <= raceInput.time; holdTimeAndSpeed++)
    {
        var travelTime = raceInput.time - holdTimeAndSpeed;
        var travelDistance = travelTime * holdTimeAndSpeed;

        if (travelDistance > raceInput.distance) numOfWinningWays++;
    }

    return numOfWinningWays;
}

IEnumerable<(ulong time, ulong distance)> ParseRaceInputs(string filename)
{
    var lines = File.ReadAllLines(filename);
    var timeComponent = lines[0].AsSpan()[5..].ToString();
    var distanceComponent = lines[1].AsSpan()[9..].ToString();

    var times = timeComponent.Split(" ")
        .Where(x => x != "")
        .Select(x => ulong.Parse(x))
        .ToArray();
    var distances = distanceComponent.Split(" ")
        .Where(x => x != "")
        .Select(x => ulong.Parse(x))
        .ToArray();

    for (var i = 0; i < times.Length; i++)
    {
        yield return (times[i], distances[i]);
    }
}

(ulong time, ulong distance) ParsePart2RaceInputs(string filename)
{
    var lines = File.ReadAllLines(filename);
    var timeComponent = lines[0].AsSpan()[5..].ToString();
    var distanceComponent = lines[1].AsSpan()[9..].ToString();

    var times = timeComponent.Split(" ")
        .Where(x => x != "");
    var distances = distanceComponent.Split(" ")
        .Where(x => x != "");

    var time = string.Join("", times);
    var distance = string.Join("", distances);

    return (ulong.Parse(time), ulong.Parse(distance));
}