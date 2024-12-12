// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//Part1("sample.txt", 1);
//Part1("sample2.txt", 6);
Part1("input.txt", 25);
//Part2("sample.txt", 1);
//Part2("sample.txt", 6);
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

  var results = BlinkTextV2(text, blinks);
  //Console.WriteLine(string.Join(" ", results));

  Console.WriteLine($"Part 2 - Total number of stones: {results.Length}");
}

List<ulong> BlinkText(string text)
{
  var stones = text.Split(" ")
    .Select(ulong.Parse)
    .ToList();

  return Blink(stones);
}

ulong[] BlinkTextV2(string text, int blinks)
{
  var stones = text.Split(" ")
    .Select(ulong.Parse)
    .ToArray();

  var stoneCache = new Dictionary<ulong, ulong[]>();

  return BlinkV2(stones, blinks, stoneCache);
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

ulong[] BlinkV2(ulong[] stones, int blinks, Dictionary<ulong, ulong[]> stoneCache)
{
  if (blinks == 0) return stones;

  var results = new List<ulong>();
  foreach (var stone in stones)
  {
    if (stoneCache.ContainsKey(stone))
    {
      results.AddRange(stoneCache[stone]);
      continue;
    }

    var result = BlinkStone(stone);
    stoneCache[stone] = result;
    results.AddRange(result);
  }

  Console.WriteLine($"After {75 - blinks} blink:");
  var input = results.ToArray();
  return BlinkV2(input, blinks - 1, stoneCache); 
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
