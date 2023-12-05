// See https://aka.ms/new-console-template for more information
using System.ComponentModel.DataAnnotations;
using System.Text;

Console.WriteLine("Hello, World!");

Part1("sample.txt");
Part1("input.txt");

void Part1(string filename)
{
  var lines = File.ReadAllLines(filename);
  var map = ParseEngineSchematic(lines);
  var coordinateMap = BuildNumberCoordinateMap(lines);

  var numbers = FindAdjacentNumbers(map, coordinateMap);
  var sum = numbers.Select(x => int.Parse(x))
    .Sum();
  
  Console.WriteLine($"Sum for {filename}: {sum}");
}

HashSet<string> FindAdjacentNumbers(char[,] map, Dictionary<(int, int), string> coordinateMap)
{
  var width = map.GetLength(0);
  var height = map.GetLength(1);
  var adjacentNumbers = new HashSet<string>();
  Console.WriteLine($"Width: {width}, Height: {height}");

  for (var x = 0; x < width; x++)
  {
    for (var y = 0; y < height; y++)
    {
      var value = map[x, y];
      if (!IsSpecialCharacter(value)) continue;

      var adjacentNumberCoordinates = GetAdjacentCoordinatesWithNumbers(x, y, map);
      foreach (var coordinate in adjacentNumberCoordinates)
      {
        var number = coordinateMap[coordinate];
        if (adjacentNumbers.Contains(number)) continue;
        adjacentNumbers.Add(number);
      }
    }
  }

  return adjacentNumbers;
}

IEnumerable<(int, int)> GetAdjacentCoordinatesWithNumbers(int currentX, int currentY, char[,] map)
{
  var startX = currentX - 1 <= 0 
    ? 0 
    : currentX - 1;
  var startY = currentY - 1 <= 0
    ? 0
    : currentY - 1;
  var endX = currentX + 2 >= map.GetLength(0) - 1
    ? map.GetLength(0)
    : currentX + 2;
  var endY = currentY + 2 >= map.GetLength(1) - 1
    ? map.GetLength(1)
    : currentY + 2;
  
  for (var y = startY; y < endY; y++)
  {
    for (var x = startX; x < endX; x++)
    {
      var value = map[x, y];
      if (!IsNumericCharacter(value)) continue;

      yield return (x, y);
    }
  }
}

static bool IsNumericCharacter(char value)
{
  return value switch
  {
    '0' => true,
    '1' => true,
    '2' => true,
    '3' => true,
    '4' => true,
    '5' => true,
    '6' => true,
    '7' => true,
    '8' => true,
    '9' => true,
    _ => false
  };
}

static bool IsSpecialCharacter(char value)
{
  return value switch
  {
    '*' => true,
    '#' => true,
    '+' => true,
    '$' => true,
    _ => false
  };
}

Dictionary<(int, int), string> BuildNumberCoordinateMap(string[] lines)
{
  var x = 0;
  var y = 0;
  var sb = new StringBuilder();
  var map = new Dictionary<(int, int), string>();

  foreach (var line in lines)
  {
    foreach (var c in line)
    {
      x++;
      var letter = c.ToString();
      if (int.TryParse(letter, out var number))
      {
        sb.Append(letter);
        continue;
      }

      // Found the number now add to map for each coordinate
      var value = sb.ToString();
      if (value.Length == 0) continue;
      sb.Clear();


      for (var i = x - @value.Length - 1; i < x - 1; i++)
      {
        map.Add((i, y), @value);
      }
    }
    
    y++;
    x = 0;
  }

  return map;
}

char[,] ParseEngineSchematic(string[] lines)
{
  var width = lines[0].Length;
  var height = lines.Length;
  var map = new char[width, height];

  for (var y = 0; y < lines.Length; y++)
  {
    var line = lines[y];
    for (var x = 0; x < line.Length; x++)
    {
      map[x,y] = line[x];
    } 
  }

  return map;
}