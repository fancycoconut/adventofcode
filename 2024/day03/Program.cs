using System.Text.RegularExpressions;
using System.Text;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//Part1("sample.txt");
//Part1("input.txt");

Part2("sample2.txt");
Part2("input.txt");

// xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))
void Part1(string filename)
{
  const string pattern = @"mul\((-?\d+),\s*(-?\d+)\)";

  var raw = File.ReadAllText(filename);

  var total = 0;
  var matches = Regex.Matches(raw, pattern);
  Console.WriteLine("Number of matches: {0}", matches.Count);
  foreach (Match match in matches)
  {
    Console.WriteLine(match.Value);
    var operands = match.Value
      .Replace("mul(", "")
      .Replace(")", "")
      .Split(",");

      total += int.Parse(operands[0]) * int.Parse(operands[1]);
  }

  Console.WriteLine($"Part 1 - Total: {total}");
}

void Part2(string filename)
{
  const string pattern = @"mul\((-?\d+),\s*(-?\d+)\)|do\(\)|don't\(\)";

  var raw = File.ReadAllText(filename);

  var total = 0;
  var matches = Regex.Matches(raw, pattern);
  Console.WriteLine("Number of matches: {0}", matches.Count);

  var multiplyIsOn = true;
  foreach (Match match in matches)
  {
    if (match.Value == "do()")
    {
      Console.WriteLine("Multiply is on");

      multiplyIsOn = true;
      continue;
    }

    if (match.Value == "don't()")
    {
      Console.WriteLine("Multiply is off");

      multiplyIsOn = false;
      continue;
    }

    if (match.Value.StartsWith("mul") && multiplyIsOn)
    {
      Console.WriteLine(match.Value);

      var operands = match.Value
        .Replace("mul(", "")
        .Replace(")", "")
        .Split(",");

      total += int.Parse(operands[0]) * int.Parse(operands[1]);
    }
  }

  Console.WriteLine($"Part 2 - Total: {total}");
}

