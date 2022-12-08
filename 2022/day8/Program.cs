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
    var top = 0;
    var bottom = 0;
    var left = 0; 
    var right = 0;
    var visibleTrees = 0;

    for (var y = 0; y < height; y++)
    {
      for (var x = 0; x < width; x++)
      {
        var treeHeight = treeHeightMap[x, y];

        // if (y >= 1) top = treeHeightMap[x, y-1];
        // if (x >= 1) left = treeHeightMap[x-1, y];
        // if (x+1 < width) right = treeHeightMap[x+1,y];
        // if (y+1 < height) bottom = treeHeightMap[x,y+1];

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

        // if ((treeHeight > left && x <= width / 2) || (treeHeight > right && x >= width / 2))
        // {
        //   visibleTrees++;
        //   continue;
        // }

        // if ((treeHeight > top && x <= height / 2) || (treeHeight > bottom && y= > height / 2))
        // {
        //   visibleTrees++;
        //   continue;
        // }


        // if (treeHeight > left && treeHeight > top && treeHeight > bottom && treeHeight > right)
        // {
        //   //Console.WriteLine($"top: {top}, bottom: {bottom}, left: {left}, right: {right} current: {treeHeight}");
        //   visibleTrees++;
        //   continue;
        // }

        Console.WriteLine($"[{x},{y}] current: {treeHeight}");
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
}
