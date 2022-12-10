namespace Day10;

public class Program
{
  public static void Main(string[] args)
  {
    Console.WriteLine("Hello World!");

    var instructions = File.ReadAllLines("input.txt");
    var simulator = new CathodeRayTube(instructions);
    simulator.Execute();

    var signalStrengthSum = simulator.GetSignalStrengthSum();
    Console.WriteLine(signalStrengthSum);
  }
}