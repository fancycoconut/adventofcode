// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//Part1("sample.txt");
Part1("input.txt");

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