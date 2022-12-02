using System.IO;
using System.Linq;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var lines = File.ReadAllLines("input.txt");

var currentCalorieCount = 0;
var calorieCounts = new List<int>();
foreach (var line in lines)
{
  if (string.IsNullOrEmpty(line))
  {
    calorieCounts.Add(currentCalorieCount);
    currentCalorieCount = 0;
    continue;
  }

  currentCalorieCount += int.Parse(line);
}

calorieCounts.Add(currentCalorieCount);

var descendingCalories = calorieCounts
  .OrderByDescending(x => x)
  .ToArray();

var top3Total = 0;
for (var i = 0; i < 3; i++)
{
  top3Total += descendingCalories[i];
}

Console.WriteLine(top3Total);
