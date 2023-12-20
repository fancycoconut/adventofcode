// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Part1("sample.txt");
Part1("input.txt");
Part2("sample.txt");
Part2("input.txt");

void Part1(string filename)
{
    var lines = File.ReadLines(filename);

    var sum = 0;
    foreach (var line in lines)
    {
        var nextValue = PredictNextValue(line);
        sum += nextValue;
    }
    
    Console.WriteLine($"Part 1 - Sum of all extrapolated values: {sum}");
}

void Part2(string filename)
{
    var lines = File.ReadLines(filename);

    var sum = 0;
    foreach (var line in lines)
    {
        var firstValue = ExtrapolateLeftMostValue(line);
        sum += firstValue;
    }
    
    Console.WriteLine($"Part 2 - Sum of all extrapolated values: {sum}");
}

int PredictNextValue(string line)
{
    var initialValues = line.Split(" ")
        .Select(x => int.Parse(x))
        .ToArray();
    var lastItemFromInitialValues = initialValues.Last();

    var placeholders = PopulatePlaceholders(initialValues);

    var predictedValue = placeholders.Select(x => x.Last())
        .Sum() + lastItemFromInitialValues;

    return predictedValue;
}

int ExtrapolateLeftMostValue(string line)
{
    var initialValues = line.Split(" ")
        .Select(x => int.Parse(x))
        .ToArray();
    var firstInitialValue = initialValues.First();
    
    var placeholderLists = PopulatePlaceholders(initialValues).ToArray();
    
    var previousValue = 0;
    for (var i = placeholderLists.Length - 1; i >= 0; i--)
    {
        var addedFirstValue = placeholderLists[i].First() - previousValue;

        previousValue = addedFirstValue;
    }

    var firstValueForInitialValue = firstInitialValue - previousValue;

    return firstValueForInitialValue ;
}

List<List<int>> PopulatePlaceholders(int[] initialValues)
{
    var placeholders = new List<List<int>>();
    List<int> currentPlaceholders;
    do
    {
        currentPlaceholders = new List<int>();
        for (var i = 1; i < initialValues.Length; i++)
        {
            var placeholder = initialValues[i] - initialValues[i - 1];
            currentPlaceholders.Add(placeholder);
        }

        initialValues = currentPlaceholders.ToArray();
        placeholders.Add(currentPlaceholders);
    } while (currentPlaceholders.All(x => x == 0) == false);

    return placeholders;
}