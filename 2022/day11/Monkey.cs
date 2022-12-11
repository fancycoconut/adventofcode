using System.Collections.Generic;

namespace Day11;

public class Monkey
{
  public Queue<int> Items { get; set; } = new();

  public Func<int, int> Operation { get; set; }

  public Func<int, bool> Test { get; set; }

  public int TrueRecipientIndex { get; set; }

  public int FalseRecipientIndex { get; set; }
  public int NumberOfInspections { get; set; }
}