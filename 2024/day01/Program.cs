// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//var lines = File.ReadAllLines("sample.txt");
var lines = File.ReadAllLines("input.txt");
Part1(lines);

//lines = File.ReadAllLines("sample.txt");
lines = File.ReadAllLines("input.txt");
Part2(lines);

void Part1(string[] lines)
{
  var list1 = ParseNumbersAtPosition(lines, 0)
    .OrderBy(x => x)
    .ToArray();
  var list2 = ParseNumbersAtPosition(lines, 1)
    .OrderBy(x => x)
    .ToArray();

  var totalDistance = 0;
  for (var i = 0; i < list1.Length; i++)
  {
    var a = list1[i];
    var b = list2[i];
    var diff = Math.Abs(a - b);
    totalDistance += diff;
  }

  Console.WriteLine($"Total distance: {totalDistance}");
}

void Part2(string[] lines)
{
    var list1 = ParseNumbersAtPosition(lines, 0).ToArray();
    var list2 = ParseNumbersAtPosition(lines, 1).ToArray();

    var numberOfOccurencesMap = GetNumberOfOccurences(list2);

    var totalSimilarityScore = 0;
    foreach (var number in list1)
    {
        numberOfOccurencesMap.TryGetValue(number, out var occurences);

        var similarityScore = number * occurences;
        totalSimilarityScore += similarityScore;
    }

    Console.WriteLine($"Total similarity score: {totalSimilarityScore}");
}

Dictionary<int, int> GetNumberOfOccurences(int[] numbers)
{
    var map = new Dictionary<int, int>();

    foreach (var number in numbers)
    {
        if (map.ContainsKey(number))
        {
            map[number]++;
        }
        else
        {
            map[number] = 1;
        }
    }

    return map;
}

IEnumerable<int> ParseNumbersAtPosition(string[] lines, int position)
{
  foreach (var line in lines)
  {
    var numberParts = line.Split("   ");
    if (position == 0)
    {
      yield return int.Parse(numberParts[0]);
    }
    else
    {
      yield return int.Parse(numberParts[1]);
    }
  }
}