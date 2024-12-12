// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Part1("sample.txt", 1);
Part1("sample2.txt", 6);
Part1("input.txt", 25);

void Part1 (string filename, int blinks)
{
  var text = File.ReadAllText(filename);
  
  List<ulong> results = new List<ulong>();
  for (var i = 0; i < blinks; i++)
  {
    results = Blink(text);
    text = string.Join(" ", results);
    Console.WriteLine($"After {i+1} blink:");
    Console.WriteLine(text);
  }

  Console.WriteLine($"Part 1 - Total number of stones: {results.Count}");
}

List<ulong> Blink(string text)
{
  var stones = text.Split(" ")
    .Select(x => ulong.Parse(x));

  var result = new List<ulong>();

  foreach (var stone in stones)
  {
    var stoneAsString = stone.ToString();
    Console.WriteLine($"Stone: {stoneAsString}");
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