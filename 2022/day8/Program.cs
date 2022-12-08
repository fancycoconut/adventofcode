namespace Day8;

public class Program
{
  public static void Main(string[] args)
  {
    Console.WriteLine("Hello World!");

    var lines = File.ReadAllLines("input.txt");
    // Not 7440, 7115, 5198, 5769

    var height = lines.Length;
    var width = lines[0].Length;
    var map = GetHeightMap(lines);
    var visibleTrees = GetVisibleTrees(map, width, height);

    Console.WriteLine($"Number of visible trees: {visibleTrees}");

    var scenicScores = GetScenicScores(map, width, height)
      .ToList();

      Console.WriteLine($"Highest scenic score: {scenicScores.Select(x => x.score).Max()}");
  }

  private static int[,] GetHeightMap(string[] lines)
  {
    var height = lines.Length;
    var width = lines[0].Length;
    var map = new int[width, height];

    for (var y = 0; y < height; y++)
    {
      var characters = lines[y].AsSpan();
      for (var x = 0; x < width; x++)
      {
        var treeHeight = int.Parse(characters[x].ToString());
        map[x, y] = treeHeight;
      }
    }

    return map;
  }

  private static int GetVisibleTrees(int[,] treeHeightMap, int width, int height)
  {
    var visibleTrees = 0;

    for (var y = 0; y < height; y++)
    {
      for (var x = 0; x < width; x++)
      {
        var treeHeight = treeHeightMap[x, y];

        // Check the edges
        if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
        {
          visibleTrees++;
          continue;
        }

        var leftSideIsVisible = LeftSideIsVisible(treeHeight, treeHeightMap, x, y);
        var rightSideIsVisible = RightSideIsVisible(treeHeight, treeHeightMap, x, y, width);
        var topSideIsVisible = TopSideIsVisible(treeHeight, treeHeightMap, x, y);
        var bottomSideIsVisible = BottomSideIsVisible(treeHeight, treeHeightMap, x, y, height);

        if (leftSideIsVisible || rightSideIsVisible || topSideIsVisible || bottomSideIsVisible)
        {
          visibleTrees++;
          continue;
        }

        // Hidden trees
        //Console.WriteLine($"[{x},{y}] current: {treeHeight}");
      }
    }

    return visibleTrees;
  }

  private static bool LeftSideIsVisible(int currentTreeHeight, int[,] map, int startX, int startY)
  {
    var x = startX - 1;
    while (x >= 0)
    {
      var leftTreeHeight = map[x, startY];
      if (leftTreeHeight >= currentTreeHeight) return false;
      x--;
    }

    return true;
  }

  private static bool RightSideIsVisible(int currentTreeHeight, int[,] map, int startX, int startY, int width)
  {
    var x = startX + 1;
    while (x < width)
    {
      var rightTreeHeight = map[x, startY];
      if (rightTreeHeight >= currentTreeHeight) return false;
      x++;
    }

    return true;
  }

  private static bool TopSideIsVisible(int currentTreeHeight, int[,] map, int startX, int startY)
  {
    var y = startY - 1;
    while (y >= 0)
    {
      var topTreeHeight = map[startX, y];
      if (topTreeHeight >= currentTreeHeight) return false;
      y--;
    }

    return true;
  }

  private static bool BottomSideIsVisible(int currentTreeHeight, int[,] map, int startX, int startY, int height)
  {
    var y = startY + 1;
    while (y < height)
    {
      var bottomTreeHeight = map[startX, y];
      if (bottomTreeHeight >= currentTreeHeight) return false;
      y++;
    }

    return true;
  }

  public static IEnumerable<(int x, int y, int score)> GetScenicScores(int[,] treeHeightMap, int width, int height)
  {
    for (var y = 0; y < height; y++)
    {
      for (var x = 0; x < width; x++)
      {
        var treeHeight = treeHeightMap[x, y];

        var leftScenicScore = GetLeftScenicScore(treeHeight, treeHeightMap, x, y);
        var rightScenicScore = GetRightScenicScore(treeHeight, treeHeightMap, x, y, width);
        var topScenicScore = GetTopScenicScore(treeHeight, treeHeightMap, x, y);
        var bottomScenicScore = GetBottomScenicScore(treeHeight, treeHeightMap, x, y, height);

        yield return (x, y, leftScenicScore * rightScenicScore * topScenicScore * bottomScenicScore);
      }
    }
  }

  private static int GetLeftScenicScore(int currentTreeHeight, int[,] map, int startX, int startY)
  {
    var score = 0;
    var x = startX - 1;
    while (x >= 0)
    {
      var leftTreeHeight = map[x, startY];
      if (leftTreeHeight >= currentTreeHeight) return score + 1;

      x--;
      score++;
    }

    return score;
  }

  private static int GetRightScenicScore(int currentTreeHeight, int[,] map, int startX, int startY, int width)
  {
    var score = 0;
    var x = startX + 1;
    while (x < width)
    {
      var rightTreeHeight = map[x, startY];
      if (rightTreeHeight >= currentTreeHeight) return score + 1;

      x++;
      score++;
    }

    return score;
  }

  private static int GetTopScenicScore(int currentTreeHeight, int[,] map, int startX, int startY)
  {
    var score = 0;
    var y = startY - 1;
    while (y >= 0)
    {
      var topTreeHeight = map[startX, y];
      if (topTreeHeight >= currentTreeHeight) return score + 1;

      y--;
      score++;
    }

    return score;
  }

  private static int GetBottomScenicScore(int currentTreeHeight, int[,] map, int startX, int startY, int height)
  {
    var score = 0;
    var y = startY + 1;
    while (y < height)
    {
      var bottomTreeHeight = map[startX, y];
      if (bottomTreeHeight >= currentTreeHeight) return score + 1;

      y++;
      score++;
    }

    return score;
  }
}
