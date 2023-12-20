// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Part1("sample.txt");
Part1("input.txt");

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

int PredictNextValue(string line)
{
    var initialValues = line.Split(" ")
        .Select(x => int.Parse(x))
        .ToArray();
    var lastItemFromInitialValues = initialValues.Last();
    
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

    var predictedValue = placeholders.Select(x => x.Last())
        .Sum() + lastItemFromInitialValues;

    return predictedValue;
}