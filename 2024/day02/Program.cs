// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Part1("sample.txt");
Part1("input.txt");
Part2("sample.txt");
Part2("input.txt");

void Part1(string filename)
{
  var lines = File.ReadAllLines(filename);

  var numOfSafeReports = 0;
  foreach (var line in lines)
  {
    var levels = line.Split(' ').Select(x => int.Parse(x)).ToArray();
    var isReportSafe = IsReportSafe(levels);
    var safeValue = isReportSafe ? "Safe" : "Unsafe";

    //Console.WriteLine($"`{line}` is {safeValue}");

    numOfSafeReports += isReportSafe ? 1 : 0;
  }

  Console.WriteLine($"Part 1 - Number of safe reports: {numOfSafeReports}");
}

void Part2(string filename)
{
  var lines = File.ReadAllLines(filename);

  var numOfSafeReports = 0;
  foreach (var line in lines)
  {
    var levels = line.Split(' ').Select(x => int.Parse(x)).ToArray();
    var isReportSafe = IsReportSafeV2(levels);
    var safeValue = isReportSafe ? "Safe" : "Unsafe";

    Console.WriteLine($"`{line}` is {safeValue}");

    numOfSafeReports += isReportSafe ? 1 : 0;
  }

  Console.WriteLine($"Number of safe reports: {numOfSafeReports}");
}

bool IsReportSafe(int[] levels)
{
  var safe = true;
  var ascendingLevels = levels.OrderBy(x => x).ToArray();
  var descendingLevels = levels.OrderByDescending(x => x).ToArray();

  var increasingOrDecreasing = ArrayIsEqual(levels, ascendingLevels) || ArrayIsEqual(levels, descendingLevels);
  for (var i = 0; i < levels.Length - 1; i++)
  {
    var diff = Math.Abs(levels[i] - levels[i+1]);
    if (!(1 <= diff && diff <= 3))
    {
      safe = false;
    }
  }

  return increasingOrDecreasing && safe;
}

bool IsReportSafeV2(int[] levels)
{
  var good = false;
  for (var j = 0; j < levels.Length; j++)
  {
    var input = levels[..j].Concat(levels[(j+1)..]).ToArray();
    //Console.WriteLine(string.Join(",", input));

    var safe = true;
    var ascendingLevels = input.OrderBy(x => x).ToArray();
    var descendingLevels = input.OrderByDescending(x => x).ToArray();

    var increasingOrDecreasing = ArrayIsEqual(input, ascendingLevels) || ArrayIsEqual(input, descendingLevels);
    for (var i = 0; i < input.Length - 1; i++)
    {
      var diff = Math.Abs(input[i] - input[i+1]);
      if (!(1 <= diff && diff <= 3))
      {
        safe = false;
      }
    }

    if (increasingOrDecreasing && safe)
    {
      good = true;
    }
  }

  return good;  
}

bool ArrayIsEqual(int[] arr1, int[] arr2)
{
  for (var i = 0; i < arr1.Length; i++)
  {
    if (arr1[i] != arr2[i]) return false;
  }

  return true;
}