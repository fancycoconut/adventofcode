namespace Day3;

public class DefaultPriorityConverter : IPriorityConverter
{
  public int GetPriority(char character)
  {
    var asciiValue = (int)character;
    // a-z 1-26
    if (97 <= asciiValue && asciiValue <= 122)
    {
      return asciiValue - 96;
    }
    else
    {
      // A-Z 27-52
      return (asciiValue - 64) + 26;
    }
  }
}