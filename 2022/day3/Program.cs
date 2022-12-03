namespace Day3;

public class Program
{
  static int Main(string[] args)
  {
    Console.WriteLine("Hello, World!");

    var rucksack = new Rucksack();
    rucksack.ReadItems("input.txt");
    var sum = rucksack.GetPrioritySum();
    var sum2 = rucksack.GetPrioritySum2();

    Console.WriteLine(sum);
    Console.WriteLine(sum2);

    return 0;
  }
}
