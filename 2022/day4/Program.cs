namespace Day4;

public class Program
{
  public static void Main(string[] args)
  {
    Console.WriteLine("Hello, World!");

    var manager = new ElfAssignmentManager();
    manager.Load("input.txt");
    var fullyCoveredAssignments = manager.CalculateFullyCoveredAssignments();
    var partialOverlappingAssignments = manager.CalculateOverlappingAssignments();

    Console.WriteLine(fullyCoveredAssignments);
    Console.WriteLine(partialOverlappingAssignments);
  }
}
