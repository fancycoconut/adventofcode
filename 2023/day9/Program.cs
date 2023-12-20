// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

void Part1(string filename)
{
    var lines = File.ReadLines(filename);
}

int PredictNextValue(string line)
{
    var predictedValues = new List<int>();
    
    var initialValues = line.Split(" ")
        .Select(x => int.Parse(x))
        .ToArray();

    var placeholders = new List<int>();
    while (true)
    {
        for (var i = 0; i <= initialValues.Length; i++)
        {
            var placeholder = initialValues[i + 1] - initialValues[i];
        }
    }
}