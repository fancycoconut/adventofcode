// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Part1("sample.txt", 1);
Part1("sample2.txt", 6);
Part1("input.txt", 25);

void Part1 (string filename, int blinks)
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

List<ulong> BlinkText(string text)
{
  var stones = text.Split(" ")
    .Select(ulong.Parse)
    .ToList();

  return Blink(stones);
}

List<ulong> Blink(List<ulong> stones)
{
  var result = new List<ulong>();

  foreach (var stone in stones)
  {
    var stoneAsString = stone.ToString();
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