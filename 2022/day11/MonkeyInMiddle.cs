namespace Day11;

public class MonkeyInMiddle
{
  private long superModulus = 1;
  private Monkey[] monkeys;

  public void Load(string input)
  {
    var lines = File.ReadAllLines(input);
    monkeys = LoadMonkeys(lines).ToArray();
  }

  private IEnumerable<Monkey> LoadMonkeys(string[] lines)
  {
    for (var i = 0; i < lines.Length; i += 7)
    {
      var end = i+6;
      var section = lines[i..end];
      var monkey = Parse(section);
      yield return monkey;
    }
  }

  private Monkey Parse(string[] lines)
  {
    var divisibleBy = long.Parse(lines[3][21..^0]);
    superModulus *= divisibleBy;

    return new Monkey {
      Items = GetItems(lines[1]),
      Operation = GetOperation(lines[2]),
      Test = x => x % divisibleBy == 0,
      TrueRecipientIndex = long.Parse(lines[4][29..^0]),
      FalseRecipientIndex = long.Parse(lines[5][30..^0])
    };
  }

  private Queue<long> GetItems(string rawItems)
  {
    var queue = new Queue<long>();
    var items = rawItems[18..^0]
        .Split(", ")
        .Select(x => long.Parse(x));
    
    foreach (var item in items)
    {
      queue.Enqueue(item);
    }

    return queue;
  }

  private Func<long, long> GetOperation(string operation)
  {
    var operand = operation[25..^0];
    var operandIsSameAsInput = operand == "old";
    if (operation.Contains("+"))
    {      
      return operandIsSameAsInput 
        ? x => x + x
        : x => x + long.Parse(operand);
    }

    return operandIsSameAsInput
      ? x => x * x
      : x => x * long.Parse(operand);
  }

  public void Part1Predict(int numberOfRounds)
  {
    for (var i = 0; i < numberOfRounds; i++)
    {
      for (var j = 0; j < monkeys.Length; j++)
      {
        var monkey = monkeys[j];
        while (monkey.Items.Any())
        {
          var item = monkey.Items.Dequeue();
          var itemWithWorryLevel = monkey.Operation(item) / 3;
          var test = monkey.Test(itemWithWorryLevel);
          if (test)
            monkeys[monkey.TrueRecipientIndex].Items.Enqueue(itemWithWorryLevel);
          else
            monkeys[monkey.FalseRecipientIndex].Items.Enqueue(itemWithWorryLevel);

          monkey.NumberOfInspections++;
        }
      }
    }
  }

  public void Part2Predict(int numberOfRounds)
  {
    for (var i = 0; i < numberOfRounds; i++)
    {
      for (var j = 0; j < monkeys.Length; j++)
      {
        var monkey = monkeys[j];
        while (monkey.Items.Any())
        {
          var item = monkey.Items.Dequeue();
          var itemWithWorryLevel = monkey.Operation(item) % superModulus;
          var test = monkey.Test(itemWithWorryLevel);
          if (test)
            monkeys[monkey.TrueRecipientIndex].Items.Enqueue(itemWithWorryLevel);
          else
            monkeys[monkey.FalseRecipientIndex].Items.Enqueue(itemWithWorryLevel);

          monkey.NumberOfInspections++;
        }
      }
    }
  }

  public long GetMonkeyBusinessLevel()
  {
    var mostActiveMonkeys = monkeys.OrderByDescending(x => x.NumberOfInspections)
      .ToArray();

    return mostActiveMonkeys[0].NumberOfInspections * mostActiveMonkeys[1].NumberOfInspections;
  }
}
