using System;
using System.Text;
using System.Linq;
using System.Collections;
using System.Text.RegularExpressions;

namespace Day5;

public class Cargo
{
  private int numberOfStacks;
  private Stack[] crateStacks;
  private string[] instructions;

  public void Load(string input)
  {
    var lines = File.ReadAllLines(input);
    numberOfStacks = lines.Single(x => x.StartsWith(" 1"))
      .Split("   ")
      .Count();

    crateStacks = InitCrateStacks(numberOfStacks);
    instructions = lines.Where(x => x.StartsWith("move"))
      .ToArray();

    ReadStackContents(lines);

    Console.WriteLine($"Number of stacks: {numberOfStacks}");
    Console.WriteLine($"Number of instructions: {instructions.Count()}");
  }

  public void MoveCratesPerCrateMover9000Instructions()
  {
    foreach (var instruction in instructions)
    {
      var parts = instruction.Split(" ");
      var amount = int.Parse(parts[1]);
      var source = int.Parse(parts[3]) - 1;
      var destination = int.Parse(parts[5]) - 1;
      
      for (var i = 0; i < amount; i++)
      {
        var item = crateStacks[source].Pop();
        crateStacks[destination].Push(item);

        //Console.WriteLine($"Moved {item} from {source} to {destination}");
      }
    }
  }

  public void MoveCratesPerCrateMover9001Instructions()
  {
    foreach (var instruction in instructions)
    {
      var parts = instruction.Split(" ");
      var amount = int.Parse(parts[1]);
      var source = int.Parse(parts[3]) - 1;
      var destination = int.Parse(parts[5]) - 1;

      if (amount == 1)
      {
        var item = crateStacks[source].Pop();
        crateStacks[destination].Push(item);
      }
      else
      {
        var orderedItems = new List<string>();
        for (var i = 0; i < amount; i++)
        {
          var item = crateStacks[source].Pop() as string;
          orderedItems.Add(item);
        }

        orderedItems.Reverse();
        orderedItems.ForEach(item => crateStacks[destination].Push(item));
      }
    }
  }

  public string PeekCratesTop()
  {
    var sb = new StringBuilder();
    foreach (var stack in crateStacks)
    {
      sb.Append(stack.Peek());
    }

    return sb.ToString();
  }

  private Stack[] InitCrateStacks(int numberOfStacks)
  {
    var crateStacks = new Stack[numberOfStacks];
    for (var i = 0; i < numberOfStacks; i++)
    {
      crateStacks[i] = new Stack();
    }

    return crateStacks;
  }

  private void ReadStackContents(string[] lines)
  {
    var stackNumberItem = lines.Single(x => x.StartsWith(" 1"));
    var indexOfStackNumbers = Array.IndexOf(lines, stackNumberItem);

    var stackContentLines = lines.Take(indexOfStackNumbers)
      .Reverse();
    foreach (var stackContent in stackContentLines)
    {
      foreach (var content in ParseStackContent(stackContent))
      {
        crateStacks[content.Item1-1].Push(content.Item2);
        Console.WriteLine($"Set {content.Item2} to crate {content.Item1}");
      }
    }
  }

  private IEnumerable<(int crateStackIndex, string value)> ParseStackContent(string content)
  {
    var regex = new Regex(@"[A-Z]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    var stackIndex = 0;
    var crateSizeLength = 4;
    var currentLinePosition = 0;
    foreach (var character in content)
    {
      if (currentLinePosition % crateSizeLength == 0)
      {
        stackIndex++;
      }

      var crate = character.ToString();
      var match = regex.Match(crate);
      if (match.Success)
      {
        yield return (stackIndex, crate);
      }

      currentLinePosition++;
    }
  }
}