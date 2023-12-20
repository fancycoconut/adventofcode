// See https://aka.ms/new-console-template for more information

using System.Numerics;

Console.WriteLine("Hello, World!");

Part1("sample.txt");
Part1("sample2.txt");
Part1("input.txt");

//Part2("part2sample.txt");
Part2("input.txt");

void Part1(string filename)
{
    var instruction = GetInstruction(filename);
    var networkMap = BuildNetworkMap(filename);

    var numOfSteps = CalculateNumberOfStepsForDestination(instruction, networkMap);

    Console.WriteLine($"Part 1 - Number of steps to reach ZZZ: {numOfSteps}");
}

void Part2(string filename)
{
    var instruction = GetInstruction(filename);
    var networkMap = BuildNetworkMap(filename);

    var startingNodes = networkMap.Keys.Where(x => x.EndsWith('A'));
    
    var multiples = startingNodes
        .Select(node => GetMultiplesOfEndingNodes(node, instruction, networkMap))
        .ToList();

    var leastCommonMultiple = multiples.LeastCommonMultiple();

    //var numOfSteps = CalculateNumberOfSimultaneousStepsForDestination(instruction, networkMap);

    Console.WriteLine($"Part 2 - Number of steps to simultaneously reach all nodes that end with Z: {leastCommonMultiple}");
}

ulong GetMultiplesOfEndingNodes(string startNode, string instruction, Dictionary<string, (string, string)> map)
{
    ulong steps = 0;
    var currentPosition = startNode;
    while (true)
    {
        foreach (var direction in instruction)
        {
            if (direction == 'L')
                currentPosition = map[currentPosition].Item1;
            else
                currentPosition = map[currentPosition].Item2;
            
            steps++;
            if (currentPosition.EndsWith('Z')) return steps;
        }
    }
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

public static class MathHelpers
{
    public static T GreatestCommonDivisor<T>(T a, T b) where T : INumber<T>
    {
        while (b != T.Zero)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    public static T LeastCommonMultiple<T>(T a, T b) where T : INumber<T>
        => a / GreatestCommonDivisor(a, b) * b;

    public static T LeastCommonMultiple<T>(this IEnumerable<T> values) where T : INumber<T>
        => values.Aggregate(LeastCommonMultiple);
}