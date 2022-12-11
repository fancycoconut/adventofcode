namespace Day11;

public class Monkey
{
  public Queue<long> Items { get; set; } = new();

  public Func<long, long> Operation { get; set; }
  public Func<long, bool> Test { get; set; }

  public long TrueRecipientIndex { get; set; }

  public long FalseRecipientIndex { get; set; }
  public long NumberOfInspections { get; set; }
}