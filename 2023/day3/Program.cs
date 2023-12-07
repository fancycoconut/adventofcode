// See https://aka.ms/new-console-template for more information
using System.ComponentModel.DataAnnotations;
using System.Text;

Console.WriteLine("Hello, World!");

Part1("sample.txt");
Part1("input.txt");
Part2("sample.txt");
Part2("input.txt");

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

void Part2(string filename)
{
  var lines = File.ReadAllLines(filename);
  var map = ParseEngineSchematic(lines);
  var coordinateMap = BuildNumberCoordinateMap(lines);
  
  var ratios = FindGearRatios(map, coordinateMap);
  var sum = ratios.Sum();
  
  Console.WriteLine($"Gear ratio sum for {filename}: {sum}");
}

List<int> FindGearRatios(char[,] map, Dictionary<(int, int), string> coordinateMap)
{
  var output = new List<int>();
  var width = map.GetLength(0);
  var height = map.GetLength(1);
  
  for (var x = 0; x < width; x++)
  {
    for (var y = 0; y < height; y++)
    {
      var value = map[x, y];
      if (value != '*') continue;

      var adjacentNumbers = new HashSet<string>();
      var adjacentNumberCoordinates = GetAdjacentCoordinatesWithNumbers(x, y, map);
      
      foreach (var coordinate in adjacentNumberCoordinates)
      {
        var number = coordinateMap[coordinate];
        if (adjacentNumbers.Contains(number)) continue;
        adjacentNumbers.Add(number);
      }
      
      if (adjacentNumbers.Count != 2) continue;

      var partNumbers = adjacentNumbers.Select(x => int.Parse(x))
        .ToArray();
      output.Add(partNumbers[0] * partNumbers[1]);
    }
  }

  return output;
}

List<string> FindAdjacentNumbers(char[,] map, Dictionary<(int, int), string> coordinateMap)
{
  var output = new List<string>();
  var width = map.GetLength(0);
  var height = map.GetLength(1);

  //Console.WriteLine($"Width: {width}, Height: {height}");

  for (var x = 0; x < width; x++)
  {
    for (var y = 0; y < height; y++)
    {
      var value = map[x, y];
      if (!IsSpecialCharacter(value)) continue;

      var adjacentNumbers = new HashSet<string>();
      var adjacentNumberCoordinates = GetAdjacentCoordinatesWithNumbers(x, y, map);
      foreach (var coordinate in adjacentNumberCoordinates)
      {
        var number = coordinateMap[coordinate];
        if (adjacentNumbers.Contains(number)) continue;
        adjacentNumbers.Add(number);
        output.Add(number);
      }
    }
  }

  return output;
}

IEnumerable<(int, int)> GetAdjacentCoordinatesWithNumbers(int currentX, int currentY, char[,] map)
{
  var leftX = currentX - 1 <= 0
    ? 0
    : currentX - 1;
  var rightX = currentX + 1 >= map.GetLength(0)
    ? map.GetLength(0) - 1
    : currentX + 1;
  var topY = currentY - 1 <= 0
    ? 0
    : currentY - 1;
  var bottomY = currentY + 1 >= map.GetLength(1)
    ? map.GetLength(1) - 1
    : currentY + 1;
  
  var topLeft = map[leftX, topY];
  if (IsNumericCharacter(topLeft)) yield return (leftX, topY);
  var topCenter = map[currentX, topY];
  if (IsNumericCharacter(topCenter)) yield return (currentX, topY);
  var topRight = map[rightX, topY];
  if (IsNumericCharacter(topRight)) yield return (rightX, topY);
  var middleLeft = map[leftX, currentY];
  if (IsNumericCharacter(middleLeft)) yield return (leftX, currentY);
  var middleCenter = map[currentX, currentY];
  if (IsNumericCharacter(middleCenter)) yield return (currentX, currentY);
  var middleRight = map[rightX, currentY];
  if (IsNumericCharacter(middleRight)) yield return (rightX, currentY);
  var bottomLeft = map[leftX, bottomY];
  if (IsNumericCharacter(bottomLeft)) yield return (leftX, bottomY);
  var bottomCenter = map[currentX, bottomY];
  if (IsNumericCharacter(bottomCenter)) yield return (currentX, bottomY);
  var bottomRight = map[rightX, bottomY];
  if (IsNumericCharacter(bottomRight)) yield return (rightX, bottomY);
}

static bool IsSpecialCharacter(char value)
{
  return value switch
  {
    '*' => true,
    '#' => true,
    '+' => true,
    '-' => true,
    '$' => true,
    '/' => true,
    '@' => true,
    '%' => true,
    '&' => true,
    '=' => true,
    _ => false
  };
}

Dictionary<(int, int), string> BuildNumberCoordinateMap(string[] lines)
{
  var x = 0;
  var y = 0;
  var sb = new StringBuilder();
  var coordinates = new List<(int, int)>();
  var map = new Dictionary<(int, int), string>();

  foreach (var line in lines)
  {
    foreach (var c in line)
    {
      x++;
      if (IsNumericCharacter(c))
      {
        sb.Append(c);
        coordinates.Add((x - 1, y));
        continue;
      }
      
      // Found the number now add to map for each coordinate
      var value = sb.ToString();
      sb.Clear();

      foreach (var coordinate in coordinates)
      {
        map.Add(coordinate, value);
      }

      coordinates.Clear();
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