// See https://aka.ms/new-console-template for more information
using System.Linq;

Console.WriteLine("Hello, World!");

Part1("sample.txt", 1);
Part1("sample2.txt", 6);
Part1("input.txt", 25);

Part2("sample.txt", 1);
Part2("sample2.txt", 6);
Part2("input.txt", 25);
Part2("input.txt", 75);

void Part1(string filename, int blinks)
{
  var text = File.ReadAllText(filename);

  var results = BlinkText(text);
  for (var i = 1; i < blinks; i++)
  {
    results = Blink(results);
    //Console.WriteLine($"After {i+1} blink:");
    //Console.WriteLine(string.Join(" ", results));
  }

  Console.WriteLine($"Part 1 - Total number of stones: {results.Count}");
}

void Part2(string filename, int blinks)
{
  var text = File.ReadAllText(filename);

  var total = BlinkTextV2(text, blinks);
  //Console.WriteLine(string.Join(" ", results));

  Console.WriteLine($"Part 2 - Total number of stones: {total}");
}

List<ulong> BlinkText(string text)
{
  var stones = text.Split(" ")
    .Select(ulong.Parse)
    .ToList();

  return Blink(stones);
}

long BlinkTextV2(string text, int blinks)
{
  var stones = text.Split(" ")
    .Select(ulong.Parse)
    .ToList();

  var countCache = new Dictionary<(ulong, int), long>();

  long total = 0;
  foreach (var stone in stones)
  {
    total += BlinkStone(stone, blinks, countCache);
  }

  return total;
}

List<ulong> Blink(List<ulong> stones)
{
  var result = new List<ulong>();

  foreach (var stone in stones)
  {
    var stoneAsString = stone.ToString().AsSpan();
    //Console.WriteLine($"Stone: {stoneAsString}");
    if (stone == 0)
    {
      result.Add(1);
      continue;
    }

    if (stoneAsString.Length % 2 == 0)
    {
      var midPoint = stoneAsString.Length / 2;
      var left = ulong.Parse(stoneAsString[0..midPoint]);
      var right = ulong.Parse(stoneAsString[midPoint..]);
      result.Add(left);
      result.Add(right);
      continue;
    }

    result.Add(stone * 2024);
  }

  return result;
}

long BlinkStone(ulong stone, int blinks, Dictionary<(ulong stone, int blinks), long> countCache)
{
  if (blinks == 0) return 1;
  if (countCache.TryGetValue((stone, blinks), out var result)) return result;
  if (stone == 0) return BlinkStone(1, blinks - 1, countCache);
  
  var numDigits = (int)Math.Floor(Math.Log10(stone)) + 1;
  if (numDigits % 2 == 0)
  {
    var split = (ulong)Math.Pow(10, numDigits / 2);
    var left = stone / split;
    var right = stone % split;
    return BlinkStone(left, blinks - 1, countCache) + BlinkStone(right, blinks - 1, countCache);
  }
  
  var value = BlinkStone(stone * 2024, blinks - 1, countCache);
  countCache[(stone, blinks)] = value;
  return value;
}