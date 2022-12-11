namespace Day11;

public class Program
{
  public static void Main(string[] args)
  {
    Console.WriteLine("Hello World!");

    var monkeyInMiddle = new MonkeyInMiddle();
    monkeyInMiddle.Load("input.txt");
    //monkeyInMiddle.Part1Predict(1);

    //var monkeyBusinessLevel = monkeyInMiddle.GetMonkeyBusinessLevel();
    //Console.WriteLine(monkeyBusinessLevel);

    monkeyInMiddle.Part2Predict(10000);
    var monkeyBusinessLevel = monkeyInMiddle.GetMonkeyBusinessLevel();
    Console.WriteLine(monkeyBusinessLevel);
  }
}