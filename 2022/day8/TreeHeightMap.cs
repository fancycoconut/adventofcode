namespace Day8;

public class TreeHeightMap
{
  public int Width { get; set; }
  public int Height { get; set; }

  private readonly int[,] map;

  private TreeHeightMap(int[,] map)
  {
    this.map = map;
  }

  public static TreeHeightMap CreateFrom(string input)
  {
    var lines = File.ReadAllLines(input);

    var height = lines.Length;
    var width = lines[0].Length;
    var map = GetHeightMap(lines, width, height);

    return new TreeHeightMap(map)
    {
      Width = width,
      Height = height
    };
  }

  private static int[,] GetHeightMap(string[] lines, int width, int height)
  {
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

  public int GetVisibleTrees()
  {
    var visibleTrees = 0;

    for (var y = 0; y < Height; y++)
    {
      for (var x = 0; x < Width; x++)
      {
        var treeHeight = map[x, y];

        // Check the edges
        if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
        {
          visibleTrees++;
          continue;
        }

        var leftSideIsVisible = LeftSideIsVisible(treeHeight, x, y);
        var rightSideIsVisible = RightSideIsVisible(treeHeight, x, y);
        var topSideIsVisible = TopSideIsVisible(treeHeight, x, y);
        var bottomSideIsVisible = BottomSideIsVisible(treeHeight, x, y);

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

  private bool LeftSideIsVisible(int currentTreeHeight, int startX, int startY)
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

  private bool RightSideIsVisible(int currentTreeHeight, int startX, int startY)
  {
    var x = startX + 1;
    while (x < Width)
    {
      var rightTreeHeight = map[x, startY];
      if (rightTreeHeight >= currentTreeHeight) return false;
      x++;
    }

    return true;
  }

  private bool TopSideIsVisible(int currentTreeHeight, int startX, int startY)
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

  private bool BottomSideIsVisible(int currentTreeHeight, int startX, int startY)
  {
    var y = startY + 1;
    while (y < Height)
    {
      var bottomTreeHeight = map[startX, y];
      if (bottomTreeHeight >= currentTreeHeight) return false;
      y++;
    }

    return true;
  }

  public IEnumerable<(int x, int y, int score)> GetScenicScores()
  {
    for (var y = 0; y < Height; y++)
    {
      for (var x = 0; x < Width; x++)
      {
        var treeHeight =  map[x, y];

        var leftScenicScore = GetLeftScenicScore(treeHeight, x, y);
        var rightScenicScore = GetRightScenicScore(treeHeight, x, y);
        var topScenicScore = GetTopScenicScore(treeHeight, x, y);
        var bottomScenicScore = GetBottomScenicScore(treeHeight, x, y);

        yield return (x, y, leftScenicScore * rightScenicScore * topScenicScore * bottomScenicScore);
      }
    }
  }

  private int GetLeftScenicScore(int currentTreeHeight, int startX, int startY)
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

  private int GetRightScenicScore(int currentTreeHeight, int startX, int startY)
  {
    var score = 0;
    var x = startX + 1;
    while (x < Width)
    {
      var rightTreeHeight = map[x, startY];
      if (rightTreeHeight >= currentTreeHeight) return score + 1;

      x++;
      score++;
    }

    return score;
  }

  private int GetTopScenicScore(int currentTreeHeight, int startX, int startY)
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

  private int GetBottomScenicScore(int currentTreeHeight, int startX, int startY)
  {
    var score = 0;
    var y = startY + 1;
    while (y < Height)
    {
      var bottomTreeHeight = map[startX, y];
      if (bottomTreeHeight >= currentTreeHeight) return score + 1;

      y++;
      score++;
    }

    return score;
  }
}