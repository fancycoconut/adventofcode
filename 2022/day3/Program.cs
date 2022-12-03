namespace Day3;

public class Program
{
  static int Main(string[] args)
  {
    Console.WriteLine("Hello, World!");

    var rucksack = new Rucksack();
    rucksack.ReadItems("input.txt");
    var sum = rucksack.GetPrioritySum();

    Console.WriteLine(sum);

    return 0;
  }
}
