namespace Day12;

public class HillClimbing
{
  private int width;
  private int height;
  private char[,] heightMap;

  private (int x, int y) startPosition;
  private (int x, int y) bestSignalPosition;

  public void Load(string data)
  {
    var lines = File.ReadAllLines(data);
    width = lines[0].Length;
    height = lines.Length;

    heightMap = LoadHeightMap(lines);
    startPosition = FindPosition(lines, "S");
    bestSignalPosition = FindPosition(lines, "E");
  }

  private char[,] LoadHeightMap(string[] lines)
  {
    var map = new char[width, height];
    for (var x = 0; x < width; x++)
    {
      for (var y = 0; y < height; y++)
      {
        var line = lines[y].AsSpan();
        map[x, y] = line[x];
      }
    }

    return map;
  }

  private (int x, int y) FindPosition(string[] lines, string character)
  {
    var i = 0;
    foreach (var line in lines)
    {      
      var indexOf = line.IndexOf(character);
      if (indexOf != -1)
      {
        return (indexOf, i);
      }
      i++;
    }

    throw new InvalidOperationException($"Unable to find start position for {character}");
  }

  // public IEnumerable<(bool, int)> GetPaths()
  // {
  //   var numberOfSteps = 0;
  //   var canContinue = true;

  //   while (canContinue)
  //   {

  //   }
  // }

  public int SolveShortestPath()
  {
    var queue = new Queue<(int x, int y)>();
    var visitedLocations = new HashSet<(int x, int y)>();
    queue.Enqueue((startPosition.x, startPosition.y));

    while (queue.Any())
    {
      var coords = queue.Dequeue();
      var x = coords.x;
      var y = coords.y;

      if (CanMoveUp(x, y-1) && Traversable((x, y), (x, y-1), visitedLocations, Part1Rule))
      {
        queue.Enqueue((x, y-1));
      }

      if (CanMoveDown(x, y+1) && Traversable((x, y), (x, y+1), visitedLocations, Part1Rule))
      {
        queue.Enqueue((x, y+1));
      }

      if (CanMoveLeft(x-1, y) && Traversable((x, y), (x-1, y), visitedLocations, Part1Rule))
      {
        queue.Enqueue((x-1, y));
      }

      if (CanMoveRight(x+1, y) && Traversable((x, y), (x+1, y), visitedLocations, Part1Rule))
      {
        queue.Enqueue((x+1, y));
      }

      visitedLocations.Add((x, y));
      var currentHeight = heightMap[x, y];
      if (currentHeight == 'E') break;

      Console.WriteLine($"({x},{y}) - {currentHeight}");
    }

    return visitedLocations.Count();
  }

  private bool Part1Rule(char f, char t) => f - t >= -1;

  private bool Traversable((int x, int y) from, (int x, int y) to, HashSet<(int, int)> visitedLocations, Func<char, char, bool> rule)
  {
    var fromHeight = heightMap[from.x, from.y] switch { 'S' => 'a', 'E' => 'z', _ => heightMap[from.x, from.y] };
    var toHeight = heightMap[to.x, to.y] switch { 'S' => 'a', 'E' => 'z', _ => heightMap[to.x, to.y] };
    return Part1Rule(fromHeight, toHeight) && !visitedLocations.Contains((to.x, to.y));
  }

  private bool CanMoveUp(int x, int y)
  {
    return y > 0;
  }

  private bool CanMoveDown(int x, int y)
  {
    return y <= height;
  }

  private bool CanMoveLeft(int x, int y)
  {
    return x > 0;
  }

  private bool CanMoveRight(int x, int y)
  {
    return x <= width;
  }

  public int FindBestSignalPath()
  {
    var visitedLocations = new HashSet<(int, int)>();
    return FindShortestPathToSignal(startPosition.x , startPosition.y, 0, 0, visitedLocations, true);
  }

  private int FindShortestPathToSignal(int x, int y, int prevHeight, int numberOfSteps, HashSet<(int,int)> visitedLocations, bool initialStart)
  {
    if (x < 0 || y < 0 || x >= width || y >= height) return 0;
    if (visitedLocations.Contains((x, y))) return 0;

    var rawHeight = heightMap[x, y];
    if (rawHeight == 'E') return numberOfSteps;
    if (!initialStart && rawHeight == 'S') return 0;

    var currentHeight = initialStart ? (int)'a' : (int)rawHeight;
    if (!initialStart && (prevHeight - currentHeight > 1)) return 0;

    visitedLocations.Add((x, y));
    Console.WriteLine($"({x},{y}) - {(char)currentHeight}");
    var i = FindShortestPathToSignal(x, y-1, currentHeight, numberOfSteps + 1, visitedLocations, false);
    var j = FindShortestPathToSignal(x, y+1, currentHeight, numberOfSteps + 1, visitedLocations, false);
    var k = FindShortestPathToSignal(x-1, y, currentHeight, numberOfSteps + 1, visitedLocations, false);
    var l = FindShortestPathToSignal(x+1, y, currentHeight, numberOfSteps + 1, visitedLocations, false);

    return i + j + k + l;
  }
}