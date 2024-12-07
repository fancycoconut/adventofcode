// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

void Part1(string filename)
{
  var lines = File.ReadAllLines(filename);

  var total = 0;
  foreach (var line in lines)
  {
    var colonIndex = line.IndexOf(':');
    var testValue = int.Parse(line[0..colonIndex]);
    var numbers = line[(colonIndex + 1)..].Split(' ')
      .Select(x => int.Parse(x))
      .ToList();

    Console.WriteLine($"{testValue}");
  }

  Console.WriteLine($"Part 1 - Total: {total}");
}

