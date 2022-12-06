namespace Day6;

public class Program
{
  public static void Main(string[] args)
  {
    Console.WriteLine("Hello World!");

    // Part 1
    {
      // Samples
      var lines = File.ReadAllLines("sample.txt");
      foreach (var line in lines)
      {
        var parser = new ElfProtocol();
        parser.Load(line);
        var position = parser.GetStartOfPacketMarkerPosition();
        Console.WriteLine($"{line} -> {position}");
      }

      // Actual
      var data = File.ReadAllText("input.txt");
      var parser2 = new ElfProtocol();
      parser2.Load(data);
      Console.WriteLine(parser2.GetStartOfPacketMarkerPosition());
    }    

    // Part 2
    {
      // Samples
      var lines = File.ReadAllLines("sample.txt");
      foreach (var line in lines)
      {
        var parser = new ElfProtocol();
        parser.Load(line);
        var position = parser.GetStartOfMessageMarkerPosition();
        Console.WriteLine($"{line} -> {position}");
      }

      // Actual
      var data = File.ReadAllText("input.txt");
      var parser2 = new ElfProtocol();
      parser2.Load(data);
      Console.WriteLine(parser2.GetStartOfMessageMarkerPosition());
    }
  }
}
