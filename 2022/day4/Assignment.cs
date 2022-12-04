using System;

namespace Day4;

// Elf assignment looks like this:
// 1-3 => 1, 2, 3
// 5-9 => 5, 6, 7, 8, 9
public class Assignment
{
  private readonly string range;

  public Assignment(string range)
  {
    this.range = range;
  }

  public int[] ToArray()
  {
    var startEndPairs = range.Split('-');
    var start = Convert.ToInt32(startEndPairs[0]);
    var end = Convert.ToInt32(startEndPairs[1]);
    return GetValues(start, end).ToArray();
  }

  private IEnumerable<int> GetValues(int start, int end)
  {
    var current = start;
    var length = end - start + 1;
    for (var i = 0; i < length; i++)
    {
      yield return current++;
    }
  }
}