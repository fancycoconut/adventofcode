// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Part1("sample.txt");
Part1("sample2.txt");
Part1("input.txt");

void Part1(string filename)
{
    var instruction = GetInstruction(filename);
    var networkMap = BuildNetworkMap(filename);

    var numOfSteps = CalculateNumberOfStepsForDestination(instruction, networkMap);

    Console.WriteLine($"Part 1 - Number of steps to reach ZZZ: {numOfSteps}");
}

int CalculateNumberOfStepsForDestination(string instruction, Dictionary<string, (string, string)> map)
{
    var steps = instruction.AsSpan();

    var numOfSteps = 0;
    var currentPosition = "AAA";
    while (currentPosition != "ZZZ")
    {
        foreach (var step in steps)
        {
            if (step == 'L')
                currentPosition = map[currentPosition].Item1;
            else
                currentPosition = map[currentPosition].Item2;

            numOfSteps++;
            if (currentPosition == "ZZZ") break;
        }
    }

    return numOfSteps;
}

string GetInstruction(string filename)
{
    var lines = File.ReadAllLines(filename);
    return lines[0];
}

Dictionary<string, (string , string)> BuildNetworkMap(string filename)
{
    var map = new Dictionary<string, (string , string)>();

    var lines = File.ReadAllLines(filename);

    for (var i = 2; i < lines.Length; i++)
    {
        var line = lines[i];
        
        var key = line.AsSpan()[0..3].ToString();
        var leftValue = line.AsSpan()[7..10].ToString();
        var rightValue = line.AsSpan()[12..15].ToString();

        map[key] = (leftValue, rightValue);
    }

    return map;
}