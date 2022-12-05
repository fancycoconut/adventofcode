namespace Day5;

public class Program
{
  public static void Main(string[] args)
  {
    Console.WriteLine("Hello, World!");

    var cargo = new Cargo();
    cargo.Load("input.txt");
    Console.WriteLine(cargo.PeekCratesTop());

    //cargo.MoveCratesPerCrateMover9000Instructions();
    cargo.MoveCratesPerCrateMover9001Instructions();

    Console.WriteLine(cargo.PeekCratesTop());
  }
}
