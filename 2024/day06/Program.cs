// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//Part1("sample.txt");
Part1("input.txt");

void Part1(string filename)
{
  var lines = File.ReadAllLines(filename);
  var mapReader = new CharMapReader(lines);

  var map = mapReader.GetMap();
  var startPosition = mapReader.FindSymbol('^');
  var totalVisitedLocations = TraverseMap(startPosition, map) + 1;

  Console.WriteLine($"Part 1 - total visited locations: {totalVisitedLocations}");
}

int TraverseMap((int x, int y) startPosition, char[,] map)
{
  var uniquePositions = new HashSet<(int , int)>();

  var currentPosition = startPosition;
  while (!WillLeaveMap(currentPosition, map))
  {
    var faceDirection = map[currentPosition.x, currentPosition.y];
    if (faceDirection == '^')
    {
      if (!CanMoveUp(currentPosition, map))
      {
        map[currentPosition.x, currentPosition.y] = '>';
        continue;
      }

      uniquePositions.Add(currentPosition);
      map[currentPosition.x, currentPosition.y] = 'X';
      currentPosition = (currentPosition.x, currentPosition.y - 1);
      map[currentPosition.x, currentPosition.y] = '^';
      continue;
    }

    if (faceDirection == 'v')
    {
      if (!CanMoveDown(currentPosition, map))
      {
        map[currentPosition.x, currentPosition.y] = '<';
        continue;
      }

      uniquePositions.Add(currentPosition);
      map[currentPosition.x, currentPosition.y] = 'X';
      currentPosition = (currentPosition.x, currentPosition.y + 1);
      map[currentPosition.x, currentPosition.y] = 'v';
      continue;
    }

    if (faceDirection == '<')
    {
      if (!CanMoveLeft(currentPosition, map))
      {
        map[currentPosition.x, currentPosition.y] = '^';
        continue;
      }

      uniquePositions.Add(currentPosition);
      map[currentPosition.x, currentPosition.y] = 'X';
      currentPosition = (currentPosition.x - 1, currentPosition.y);
      map[currentPosition.x, currentPosition.y] = '<';
      continue;
    }

    if (faceDirection == '>')
    {
      if (!CanMoveRight(currentPosition, map))
      {
        map[currentPosition.x, currentPosition.y] = 'v';
        continue;
      }

      uniquePositions.Add(currentPosition);
      map[currentPosition.x, currentPosition.y] = 'X';
      currentPosition = (currentPosition.x + 1, currentPosition.y);
      map[currentPosition.x, currentPosition.y] = '>';
      continue;
    }
  }

  return uniquePositions.Count;
}

bool CanMoveUp((int x, int y) position, char[,] map)
{
  var y = position.y - 1 < 0 ? 0 : position.y - 1;
  return map[position.x, y] != '#';
}

bool CanMoveDown((int x, int y) position, char[,] map)
{
  var height = map.GetLength(1);
  var y = position.y + 1 >= height ? height - 1 : position.y + 1;
  return map[position.x, y] != '#';
}

bool CanMoveLeft((int x, int y) position, char[,] map)
{
  var x = position.x - 1 < 0 ? 0 : position.x - 1;
  return map[x, position.y] != '#';
}

bool CanMoveRight((int x, int y) position, char[,] map)
{
  var width = map.GetLength(0);
  var x = position.x + 1 >= width ? width - 1 : position.x + 1;
  return map[x, position.y] != '#';
}

bool WillLeaveMap((int x, int y) position, char[,] map)
{
  var width = map.GetLength(0);
  var height = map.GetLength(1);
  var faceDirection = map[position.x, position.y];

  return faceDirection switch
  {
    '^' => position.y - 1 < 0,
    'v' => position.y + 1 >= height,
    '<' => position.x - 1 < 0,
    '>' => position.x + 1 >= width,
    _ => throw new InvalidOperationException("Invalid face position")
  };
}

public class CharMapReader
{
  private int _width;
  private int _height;

  private readonly char[,] _map;

  public CharMapReader(string[] lines)
  {
    _height = lines.Length;
    _width = lines[0].Length;
    _map = BuildMap(lines);
  }

  public char[,] GetMap() => _map;

  public (int x, int y) FindSymbol(char symbol)
  {
    for (var x = 0; x < _width; x++)
    {
      for (var y = 0; y < _height; y++)
      {
        if (_map[x, y] == symbol) return (x, y);
      }
    }

    throw new InvalidOperationException($"Unable to find symbol '{symbol}' in map");
  }

  private char[,] BuildMap(string[] lines)
  {
    var map = new char[_width, _height];

    var x = 0;
    var y = 0;
    foreach (var line in lines)
    {
      foreach (var letter in line)
      {
        map[x, y] = letter;
        x++;
      }

      y++;
      x = 0;
    }

    return map;
  }
}