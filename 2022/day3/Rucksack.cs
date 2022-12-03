using System.Linq;

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

  public int GetPrioritySum2()
  {
    return GetCommonAllItems3()
      .Select(item => {
        Console.WriteLine(item);
        return priorityConverter.GetPriority(char.Parse(item));
      })
      .Sum();
  }

  private IEnumerable<string> GetCommonAllItems3()
  {
    // Get common items in groups of 3
    for (var i = 0; i < rucksackItems.Length; i = i + 3)
    {
      var elfItems1 = rucksackItems[i].ToCharArray();
      var elfItems2 = rucksackItems[i+1].ToCharArray();
      var elfItems3 = rucksackItems[i+2].ToCharArray();

      var commonItem = elfItems1.Intersect(elfItems2).Intersect(elfItems3);
      yield return commonItem.First().ToString();
    }
  }

  private string GetCommonItem(string items)
  {
    var itemMap = new Dictionary<char, int>();

    var midpoint = items.Length / 2;
    var compartment1 = items.Substring(0, midpoint);
    var compartment2 = items.Substring(midpoint);

    AddItemCountsToMap(itemMap, compartment1, compartment2);

    return itemMap.First(x => x.Value > 0)
      .Key
      .ToString();
  }

  private void AddItemCountsToMap(Dictionary<char, int> itemMap, params string[] itemsOfItems)
  {
    var count = 0;
    foreach (var items in itemsOfItems)
    {
      if (count == 0)
      {
        foreach (var item in items)
        {
          if (!itemMap.ContainsKey(item))
            itemMap[item] = 0;
        }
      }
      else
      {
        foreach (var item in items)
        {
          if (itemMap.ContainsKey(item))
            itemMap[item]++;
          else
            itemMap[item] = 0;
        }
      }
      count++;
    }
  }
}