using System.Text;

namespace Day10;

public class CathodeRayTube
{
  private int cycles = 0;
  private int position = 0;
  private int registerX = 1;

  private readonly string[] instructions;
  private readonly List<int> signalStrengths = new();
  private readonly StringBuilder sb = new StringBuilder();

  public CathodeRayTube(string[] instructions)
  {
    this.instructions = instructions;
  }

  public void Execute()
  {
    foreach (var instruction in instructions)
    {
      var opcodePair = instruction.Split(" ");
      switch (opcodePair[0])
      {
        case "addx":
          var value = int.Parse(opcodePair[1]);
          IncrementCycle();
          IncrementCycle();
          registerX += value;
          break;
        case "noop":
        default:
          IncrementCycle();
          break;
      }      
    }

    Console.WriteLine();
    Console.WriteLine($"Total Cycles: {cycles}");
    Console.WriteLine($"Register X: {registerX}");
  }

  public int GetSignalStrengthSum()
  {
    return signalStrengths.Sum();
  }

  private void IncrementCycle()
  {
    cycles++;
    var signalStrength = GetSignalStrength();
    signalStrengths.Add(signalStrength);
    Render();
  }

  private int GetSignalStrength()
  {
    if (cycles == 20) return cycles * registerX;
    if (cycles == 60) return cycles * registerX;
    if (cycles == 100) return cycles * registerX;
    if (cycles == 140) return cycles * registerX;
    if (cycles == 180) return cycles * registerX;
    if (cycles == 220) return cycles * registerX;
    return 0;
  }

  private void Render()
  {
    var spritePositionedAtDrawnPixel = (position == registerX - 1 || position == registerX + 1 || position == registerX);
    
    if (spritePositionedAtDrawnPixel)
      sb.Append("#");
    else
      sb.Append(".");
    
    position++;
    if (position == 40)
    {
      Console.WriteLine(sb.ToString());
      sb.Clear();
      position = 0;
    }
  }
}
