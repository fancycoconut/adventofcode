// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//Part1("sample.txt");
Part1("input.txt");

void Part1(string filename)
{
  var lines = File.ReadAllLines(filename);

  var numOfSafeReports = 0;
  foreach (var line in lines)
  {
    var levels = line.Split(' ').Select(x => int.Parse(x)).ToArray();
    var isReportSafe = IsReportSafe(levels);
    var safeValue = isReportSafe ? "Safe" : "Unsafe";

    Console.WriteLine($"`{line}` is {safeValue}");

    numOfSafeReports += isReportSafe ? 1 : 0;
  }

  Console.WriteLine($"Number of safe reports: {numOfSafeReports}");
}

bool IsReportSafe(int[] levels)
{
  var prev = levels[0];
  var isIncreasing = false;
  var isDecreasing = false;
  for (var i = 1; i < levels.Length; i++)
  {
    var current = levels[i];
    var diff = current - prev;
    // Neither decrease or increase
    if (diff == 0) return false;

    isIncreasing = isIncreasing ? true : diff > 0;
    isDecreasing = isDecreasing ? true : diff < 0;

    if (isIncreasing && isDecreasing) return false;
    if (Math.Abs(diff) > 3) return false;

    prev = current;
  }

  return true;
}