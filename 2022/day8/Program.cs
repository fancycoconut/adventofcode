namespace Day8;

public class Program
{
  public static void Main(string[] args)
  {
    Console.WriteLine("Hello World!");

    var map = TreeHeightMap.CreateFrom("sample.txt");
    var visibleTrees = map.GetVisibleTrees();

    Console.WriteLine($"Number of visible trees: {visibleTrees}");

    var scenicScores = map.GetScenicScores()
      .ToList();

      Console.WriteLine($"Highest scenic score: {scenicScores.Select(x => x.score).Max()}");
  }
}
