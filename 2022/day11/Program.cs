namespace Day11;

public class Program
{
  public static void Main(string[] args)
  {
    Console.WriteLine("Hello World!");

    var monkeyInMiddle = new MonkeyInMiddle();
    monkeyInMiddle.Load("input.txt");
    monkeyInMiddle.Predict(20);

    var monkeyBusinessLevel = monkeyInMiddle.GetMonkeyBusinessLevel();
    Console.WriteLine(monkeyBusinessLevel);
  }
}