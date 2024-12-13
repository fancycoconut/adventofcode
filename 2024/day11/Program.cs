// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//Part1("sample.txt", 1);
Part1("sample2.txt", 6);
Part1("input.txt", 25);
//Part2("sample.txt", 1);
//Part2("sample2.txt", 6);
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

int BlinkTextV2(string text, int blinks)
{
  var stones = text.Split(" ")
    .Select(ulong.Parse);

  var countCache = new Dictionary<ulong, int>();
  return BlinkStones(stones.ToArray(), blinks, countCache);
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

int BlinkStones(ulong[] stones, int blinks, Dictionary<ulong, int> countCache)
{
  var total = 0;
  if (blinks == 1)
  {
    foreach (var stone in stones)
    {
      if (countCache.TryGetValue(stone, out var count))
      {
        total += count;
      }
      else
      {
        var results = BlinkStone(stone);
        total += results.Length;
        countCache[stone] = results.Length;
      }
    }
    return total;
  }

  foreach (var stone in stones)
  {
    var results = BlinkStone(stone);
    total += BlinkStones(results, blinks - 1, countCache);
    Console.WriteLine($"Blink: {blinks} total: {total}");
  }

  return total;
}

ulong[] BlinkStone(ulong stone)
{
  if (stone == 0) return new ulong[] { 1 };

  var numDigits = (int)Math.Floor(Math.Log10(stone)) + 1;
  if (numDigits % 2 == 0)
  {
    var split = (ulong)Math.Pow(10, numDigits / 2);
    var left = stone / split;
    var right = stone % split;
    return new ulong[] { left, right };
  }

  return new ulong[] { stone * 2024 };
}
