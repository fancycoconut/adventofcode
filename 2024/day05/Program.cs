// See https://aka.ms/new-console-template for more information
using System.Collections;
using System.Runtime.InteropServices;

Console.WriteLine("Hello, World!");

//Part1("sample.txt");
//Part1("input.txt");

Part2("sample.txt");
Part2("input.txt");

void Part1(string filename)
{
  var text = File.ReadAllText(filename);
  var sections = text.Split("\r\n\r\n");

  var rules = sections[0].Split("\r\n");
  var updates = sections[1].Split("\r\n");
  var rulesList = GetRulesList(rules);

  var total = 0;
  foreach (var update in updates)
  {
    var pageNumbers = GetUpdatePageNumbers(update);
    var isCorrect = IsUpdateInCorrectOrder(pageNumbers, rulesList);
    Console.WriteLine($"Update: `{update}` is correct? {isCorrect}");
    if (!isCorrect) continue;
    
    total += pageNumbers[pageNumbers.Length / 2];
  }
  
  Console.WriteLine($"Part 1 - total: {total}");
}

void Part2(string filename)
{
  var text = File.ReadAllText(filename);
  var sections = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) 
    ? text.Split("\r\n\r\n")
    : text.Split("\n\n");

  var rules = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) 
    ? sections[0].Split("\r\n")
    : sections[0].Split("\n");
  var updates = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) 
    ? sections[1].Split("\r\n")
    : sections[1].Split("\n");
  var rulesList = GetRulesList(rules);

  var total = 0;
  foreach (var update in updates)
  {
    var pageNumbers = GetUpdatePageNumbers(update);
    var isCorrect = IsUpdateInCorrectOrder(pageNumbers, rulesList);
    Console.WriteLine($"Update: `{update}` is correct? {isCorrect}");
    if (isCorrect) continue;
    
    var comparer = new PrinterPageCorrectionComparer(pageNumbers, rulesList);
    Array.Sort(pageNumbers, comparer);
    Console.WriteLine($"Sorted: {string.Join(",", pageNumbers)}");
    
    total += pageNumbers[pageNumbers.Length / 2];
  }
  
  Console.WriteLine($"Part 1 - total: {total}");
}

void InsertionSort(int[] pageNumbers, List<(int left, int right)> rulesList)
{
  for (var i = 0; i < pageNumbers.Length - 1; i++)
  {
    var current = pageNumbers[i];

    for (var j = 1; j < pageNumbers.Length; j++)
    {
      var next = pageNumbers[j];
       
      foreach (var rule in rulesList)
      {
        var positionOfRightInArray = Array.IndexOf(pageNumbers, rule.right);
        var rightExists = positionOfRightInArray != -1;
        if (current == rule.left && rightExists && positionOfRightInArray < i)
        {
          var temp = pageNumbers[0];
          pageNumbers[0] = current;
          pageNumbers[i] = temp;
        }
      }
    }
  }
}

bool IsUpdateInCorrectOrder(int[] pageNumbers, List<(int left, int right)> rulesList)
{
  for (var i = 0; i < pageNumbers.Length; i++)
  {
    var current = pageNumbers[i];

    foreach (var rule in rulesList)
    {
      var positionOfRightInArray = Array.IndexOf(pageNumbers, rule.right);
      var rightExists = positionOfRightInArray != -1;
      if (current == rule.left && rightExists && positionOfRightInArray < i)
      {
        return false;
      }
    }
  }
  
  return true;
}

int[] GetUpdatePageNumbers(string update)
{
  return update.Split(",")
    .Select(int.Parse)
    .ToArray();
}

List<(int left, int right)> GetRulesList(string[] rules)
{
  var rulesList = new List<(int left, int right)>();

  foreach (var rule in rules)
  {
    var ruleParts = rule.Split("|");
    rulesList.Add((int.Parse(ruleParts[0]), int.Parse(ruleParts[1])));
  }

  return rulesList;
}

public class PrinterPageCorrectionComparer : IComparer<int>
{
  private readonly int[] _pageNumbers;
  private readonly Dictionary<int, HashSet<int>> _rulesMap;

  public PrinterPageCorrectionComparer(int[] pageNumbers, List<(int left, int right)> rulesList)
  {
    _pageNumbers = pageNumbers;
    _rulesMap = BuildRulesMap(rulesList);
  }
  
  public int Compare(int left, int right)
  {
    if (left == right) return 0;
    if (IsBefore(left, right)) return -1;
    if (IsBefore(right, left)) return 1;
    return left.CompareTo(right);
  }

  private bool IsBefore(int left, int right)
  {
    if (!_rulesMap.TryGetValue(left, out var leftBeforeValues)) return false;
      
    if (leftBeforeValues.Contains(right)) return true;
    return false;
  }

  private Dictionary<int, HashSet<int>> BuildRulesMap(List<(int left, int right)> rulesList)
  {
    var map = new Dictionary<int, HashSet<int>>();

    foreach (var rule in rulesList)
    {
      if (map.ContainsKey(rule.left))
      {
        map[rule.left].Add(rule.right);
      }
      else
      {
        var set = new HashSet<int> { rule.right };
        map[rule.left] = set;
      }
    }

    return map;
  }
}