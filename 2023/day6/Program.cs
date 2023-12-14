// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Part1("sample.txt");
Part1("input.txt");

void Part1(string filename)
{
    var raceInputs = ParseRaceInputs(filename)
        .ToList();

    var raceWinningWays = new List<int>();
    foreach (var input in raceInputs)
    {
        var numOfWinningWays = CalculateNumberOfWinningWays(input);
        raceWinningWays.Add(numOfWinningWays);
    }

    var marginOfError = 1;
    foreach (var numOfWinningWays in raceWinningWays)
    {
        marginOfError *= numOfWinningWays;
    }
    
    Console.WriteLine($"Part 1 - Margin of Error: {marginOfError}");
}

int CalculateNumberOfWinningWays((int time, int distance) raceInput)
{
    var numOfWinningWays = 0; 
    for (var holdTimeAndSpeed = 0; holdTimeAndSpeed <= raceInput.time; holdTimeAndSpeed++)
    {
        var travelTime = raceInput.time - holdTimeAndSpeed;
        var travelDistance = travelTime * holdTimeAndSpeed;

        if (travelDistance > raceInput.distance) numOfWinningWays++;
    }

    return numOfWinningWays;
}

IEnumerable<(int time, int distance)> ParseRaceInputs(string filename)
{
    var lines = File.ReadAllLines(filename);
    var timeComponent = lines[0].AsSpan()[5..].ToString();
    var distanceComponent = lines[1].AsSpan()[9..].ToString();

    var times = timeComponent.Split(" ")
        .Where(x => x != "")
        .Select(x => int.Parse(x))
        .ToArray();
    var distances = distanceComponent.Split(" ")
        .Where(x => x != "")
        .Select(x => int.Parse(x))
        .ToArray();

    for (var i = 0; i < times.Length; i++)
    {
        yield return (times[i], distances[i]);
    }
}