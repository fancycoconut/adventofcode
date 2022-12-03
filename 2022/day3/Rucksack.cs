namespace Day3;

public class Rucksack
{
  private string[] rucksackItems;
  private readonly IPriorityConverter priorityConverter;

  public Rucksack()
  {
    rucksackItems = new string[0];
    priorityConverter = new DefaultPriorityConverter();
  }

  public void ReadItems(string input)
  {
    rucksackItems = File.ReadAllLines(input);
  }

  public int GetPrioritySum()
  {
    var sum = 0;
    foreach (var items in rucksackItems)
    {
      var commonItem = GetCommonItem(items);
      var priority = priorityConverter.GetPriority(char.Parse(commonItem));

      sum += priority;
    }

    return sum;
  }

  private string GetCommonItem(string items)
  {
    var itemMap = new Dictionary<char, int>();

    var midpoint = items.Length / 2;
    var compartment1 = items.Substring(0, midpoint);
    var compartment2 = items.Substring(midpoint);
    
    foreach (var item in compartment1)
    {
      if (!itemMap.ContainsKey(item))
        itemMap[item] = 0;
    }

    foreach (var item in compartment2)
    {
      if (itemMap.ContainsKey(item))
        itemMap[item]++;
      else
        itemMap[item] = 0;
    }

    return itemMap.First(x => x.Value > 0)
      .Key
      .ToString();
  }
}