// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Part1("sample.txt");
Part1("sample2.txt");
Part1("sample3.txt");
Part1("sample4.txt");
Part1("input.txt");

Part2("sample5.txt");
Part2("sample6.txt");
Part2("sample7.txt");
Part2("sample.txt");
Part2("input.txt");

void Part1(string filename)
{
  var lines = File.ReadAllLines(filename);

  var mapReader = new IntMapReader(lines);
  var map = mapReader.GetMap();
  var startPositions = GetAllTrailHeadPositions(mapReader).ToList();

  var scores = CalculateScores(startPositions, mapReader).ToList();

  Console.WriteLine($"Part 1 - {scores.Sum()}");
}

void Part2(string filename)
{
  var lines = File.ReadAllLines(filename);

  var mapReader = new IntMapReader(lines);
  var map = mapReader.GetMap();
  var startPositions = GetAllTrailHeadPositions(mapReader).ToList();

  var scores = CalculateRatings(startPositions, mapReader).ToList();

  Console.WriteLine($"Part 2 - {scores.Sum()}");
}

IEnumerable<int> CalculateScores(List<(int x, int y)> startPositions, IntMapReader mapReader)
{
  foreach (var position in startPositions)
  {
    var score = CalculateScore(position, mapReader);
    Console.WriteLine($"({position.x}, {position.y}) has score: {score}");
    yield return score;
  }
}

IEnumerable<int> CalculateRatings(List<(int x, int y)> startPositions, IntMapReader mapReader)
{
  foreach (var position in startPositions)
  {
    var score = CalculateRating(position, mapReader);
    //Console.WriteLine($"({position.x}, {position.y}) has score: {score}");
    yield return score;
  }
}

int CalculateRating((int x, int y) startPosition, IntMapReader mapReader)
{
  var map = mapReader.GetMap();
  var width = mapReader.Width;
  var height = mapReader.Height;

  var score = 0;
  var queue = new Queue<(int x, int y)>();
  queue.Enqueue(startPosition);

  while (queue.Count > 0)
  {
    var currentPosition = queue.Dequeue();

    var x = currentPosition.x;
    var y = currentPosition.y;
    var currentHeight = map[x, y];
    var nextHeight = currentHeight + 1;
    if (currentHeight == 9)
    {
      score++;
      continue;
    }

    // Can move up
    if (y - 1 >= 0)
    {
      var upHeight = map[x, y - 1];
      if (upHeight == nextHeight)
      {
        queue.Enqueue((x, y - 1));
      }
    }

    // Can move down
    if (y + 1 < height)
    {
      var downHeight = map[x, y + 1];
      if (downHeight == nextHeight)
      {
        queue.Enqueue((x, y + 1));
      }
    }

    // Can move left
    if (x - 1 >= 0)
    {
      var leftHeight = map[x - 1, y];
      if (leftHeight == nextHeight)
      {
        queue.Enqueue((x - 1, y));
      }
    }

    // Can move right
    if (x + 1 < width)
    {
      var rightHeight = map[x + 1, y];
      if (rightHeight == nextHeight)
      {
        queue.Enqueue((x + 1, y));
      }
    }
  }

  return score;
}

int CalculateScore((int x, int y) startPosition, IntMapReader mapReader)
{
  var map = mapReader.GetMap();
  var width = mapReader.Width;
  var height = mapReader.Height;

  var score = 0;
  var queue = new Queue<(int x, int y)>();
  queue.Enqueue(startPosition);

  var visitedLocations = new HashSet<(int x, int y)>();

  while (queue.Count > 0)
  {
    var currentPosition = queue.Dequeue();
    if (visitedLocations.Contains(currentPosition)) continue;

    var x = currentPosition.x;
    var y = currentPosition.y;
    var currentHeight = map[x, y];
    var nextHeight = currentHeight + 1;
    if (currentHeight == 9)
    {
      score++;
      visitedLocations.Add(currentPosition);
      continue;
    }

    // Can move up
    if (y - 1 >= 0)
    {
      var upHeight = map[x, y - 1];
      if (upHeight == nextHeight)
      {
        queue.Enqueue((x, y - 1));
      }
    }

    // Can move down
    if (y + 1 < height)
    {
      var downHeight = map[x, y + 1];
      if (downHeight == nextHeight)
      {
        queue.Enqueue((x, y + 1));
      }
    }

    // Can move left
    if (x - 1 >= 0)
    {
      var leftHeight = map[x - 1, y];
      if (leftHeight == nextHeight)
      {
        queue.Enqueue((x - 1, y));
      }
    }

    // Can move right
    if (x + 1 < width)
    {
      var rightHeight = map[x + 1, y];
      if (rightHeight == nextHeight)
      {
        queue.Enqueue((x + 1, y));
      }
    }

    //visitedLocations.Add(currentPosition);
  }

  return visitedLocations.Count;
}

IEnumerable<(int x, int y)> GetAllTrailHeadPositions(IntMapReader mapReader)
{
  var map = mapReader.GetMap();
  var width = mapReader.Width;
  var height = mapReader.Height;

  for (var x = 0; x < width; x++)
  {
    for (var y = 0; y < height; y++)
    {
      var currentValue = map[x, y];
      if (currentValue == 0) yield return (x, y);
    }
  }
}

public class IntMapReader
{
  private int _width;
  private int _height;

  private readonly int[,] _map;

  public IntMapReader(string[] lines)
  {
    _height = lines.Length;
    _width = lines[0].Length;
    _map = BuildMap(lines);
  }

  public int[,] GetMap() => _map;
  public int Width => _width;
  public int Height => _height;

  public (int x, int y) FindNumber(int @value)
  {
    for (var x = 0; x < _width; x++)
    {
      for (var y = 0; y < _height; y++)
      {
        if (_map[x, y] == @value) return (x, y);
      }
    }

    throw new InvalidOperationException($"Unable to find symbol '{@value}' in map");
  }

  private int[,] BuildMap(string[] lines)
  {
    var map = new int[_width, _height];

    var x = 0;
    var y = 0;
    foreach (var line in lines)
    {
      foreach (var letter in line)
      {
        if (int.TryParse(letter.ToString(), out var @value))
        {
          map[x, y] = @value;
        }
        else
        {
          map[x, y] = 255;
        }
        
        x++;
      }

      y++;
      x = 0;
    }

    return map;
  }
}