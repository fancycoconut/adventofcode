// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//Part1("sample.txt", 1);
//Part1("sample2.txt", 6);
//Part1("input.txt", 25);
//Part2("input.txt", 25);
Part2("input.txt", 75);

void Part1(string filename, int blinks)
{
  var text = File.ReadAllText(filename);

  var results = BlinkText(text);
  for (var i = 1; i < blinks; i++)
  {
    results = Blink(results);
    Console.WriteLine($"After {i+1} blink:");
    //Console.WriteLine(string.Join(" ", results));
  }

  Console.WriteLine($"Part 1 - Total number of stones: {results.Count}");
}

void Part2(string filename, int blinks)
{
  var text = File.ReadAllText(filename);
  var multiplyCache = new Dictionary<ulong, ulong>();
  var cache = new Dictionary<ulong, ulong>();

  var results = BlinkTextV2(text, multiplyCache);
  for (var i = 1; i < blinks; i++)
  {
    results = BlinkV2(results, multiplyCache);
    Console.WriteLine($"After {i+1} blink:");
    //Console.WriteLine(string.Join(" ", results));
  }

  Console.WriteLine($"Part 2 - Total number of stones: {results.Count}");
}

List<ulong> BlinkText(string text)
{
  var stones = text.Split(" ")
    .Select(ulong.Parse)
    .ToList();

  return Blink(stones);
}

List<ulong> BlinkTextV2(string text, Dictionary<ulong, ulong> multiplyCache)
{
  var stones = text.Split(" ")
    .Select(ulong.Parse)
    .ToList();

  return BlinkV2(stones, multiplyCache);
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


List<ulong> BlinkV2(List<ulong> stones, Dictionary<ulong, ulong> multiplyCache)
{
  var result = new List<ulong>();

  foreach (var stone in stones)
  {
    var numDigits = (int)Math.Floor(Math.Log10(stone)) + 1;
    //Console.WriteLine($"Stone: {stoneAsString}");
    if (stone == 0)
    {
      result.Add(1);
      continue;
    }

    if (numDigits % 2 == 0)
    {
      var split = (ulong)Math.Pow(10, numDigits / 2);
      var left = stone / split;
      var right = stone % split;
      result.Add(left);
      result.Add(right);
      continue;
    }

    if (multiplyCache.ContainsKey(stone))
    {
      result.Add(multiplyCache[stone]);
      continue;
    }
    else
    {
      var value = stone * 2024;
      multiplyCache[stone] = value;
      result.Add(value);
    }    
  }

  return result;
}
