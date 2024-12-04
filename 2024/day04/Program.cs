using System.Text.RegularExpressions;
using System.Text;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Part1("sample.txt");
//Part1("input.txt");

Part2("sample.txt");
Part2("input.txt");

void Part1(string filename)
{
  var lines = File.ReadAllLines(filename);
  var map = BuildMap(lines);

  var verticalLines = GetVerticalLines(map);
  var diagonalTopLeftBottomRightLines = GetTopLeftToBottomRightDiagonalLines(map);
  var diagonalTopRightBottomLeftLines = GetTopRightToBottomLeftDiagonalLines(map);

  var horizontalCount = CountLines(lines);
  var verticalCount = CountLines(verticalLines);
  var diagonalTopLeftBottomRightCount = CountLines(diagonalTopLeftBottomRightLines);
  var diagonalTopRightBottomLeftCount = CountLines(diagonalTopRightBottomLeftLines);

  Console.WriteLine($"Horizontal: {horizontalCount}");
  Console.WriteLine($"Vertical: {verticalCount}");
  Console.WriteLine($"Diagonal (top left - bottom right): {diagonalTopLeftBottomRightCount}");
  Console.WriteLine($"Diagonal (top right - bottom left): {diagonalTopRightBottomLeftCount}");

  var total = horizontalCount + verticalCount + diagonalTopLeftBottomRightCount + diagonalTopRightBottomLeftCount;
  Console.WriteLine($"Part 1 - Total: {total}");
}

void Part2(string filename)
{
  var lines = File.ReadAllLines(filename);
  var map = BuildMap(lines);

  var coordinates = FindCoordinatesForLetter(map, 'A').ToList();
  // foreach (var coordinate in coordinates)
  // {
  //   Console.WriteLine($"({coordinate.x}, {coordinate.y})");
  // }

  var total = CountForXMas(coordinates, map);

  Console.WriteLine($"Part 2 - Total: {total}");
}

int CountForXMas(List<(int x, int y)> coordinates, char[,] map)
{
  var width = map.GetLength(0);
  var height = map.GetLength(1);

  var count = 0;
  var queue = new Queue<(int x, int y)>();
  Console.WriteLine($"Total coordinates: {coordinates.Count}");
  foreach (var coordinate in coordinates)
  {
    //Console.WriteLine($"({coordinate.x},{coordinate.y})");
    queue.Enqueue(coordinate);
  }

  while (queue.Count > 0)
  {
    var coord = queue.Dequeue();
    var x = coord.x;
    var y = coord.y;
    if (x - 1 < 0 || x + 1 >= width || y - 1 < 0 || y + 1 >= height)
    {
      //Console.WriteLine($"({x},{y}) - fail");
      continue;
    }

    var topLeft = map[x-1, y-1];
    var topRight = map[x+1, y-1];
    var bottomLeft = map[x-1, y+1];
    var bottomRight = map[x+1, y+1];

    if ((topLeft == 'M' && bottomRight == 'S' && bottomLeft == 'M' && topRight == 'S')
      ||(topLeft == 'M' && bottomRight == 'S' && bottomLeft == 'S' && topRight == 'M')
      ||(topLeft == 'S' && bottomRight == 'M' && bottomLeft == 'M' && topRight == 'S')
      ||(topLeft == 'S' && bottomRight == 'M' && bottomLeft == 'S' && topRight == 'M'))
    {
      //Console.WriteLine($"({x},{y}) - pass");
      count++;
    }
  }

  return count;
}

IEnumerable<(int x, int y)> FindCoordinatesForLetter(char[,] map, char letter)
{
  var width = map.GetLength(0);
  var height = map.GetLength(1);

  for (var x = 0; x < width; x++)
  {
    for (var y = 0; y < height; y++)
    {
      if (map[x, y] == letter)
      {
        yield return (x, y);
      }
    }
  }
}

List<string> GetVerticalLines(char[,] map)
{
  var width = map.GetLength(0);
  var height = map.GetLength(1);
  var lines = new List<string>();

  var sb = new StringBuilder();
  for (var x = 0; x < width; x++)
  {
    for (var y = 0; y < height; y++)
    {
      sb.Append(map[x, y]);
    }

    lines.Add(sb.ToString());
    sb.Clear();
  }

  return lines;
}

List<string> GetTopLeftToBottomRightDiagonalLines(char[,] map)
{
  var width = map.GetLength(0);
  var height = map.GetLength(1);
  var lines = new HashSet<string>();

  // Diagonals starting from the top row
  for (var y = 0; y < height; y++)
  {
    var letters = GetDiagonal(map, 0, y, 1, 1);
    //Console.WriteLine($"1 - {string.Join("", letters)}");
    lines.Add(string.Join("", letters));
  }

  // Diagonals starting from the leftmost column (excluding top corner)
  for (var x = 1; x < width; x++)
  {
    var letters = GetDiagonal(map, x, 0, 1, 1);
    //Console.WriteLine($"2 - {string.Join("", letters)}");
    lines.Add(string.Join("", letters));
  }

  return lines.ToList();
}

List<string> GetTopRightToBottomLeftDiagonalLines(char[,] map)
{
  var width = map.GetLength(0);
  var height = map.GetLength(1);
  var lines = new HashSet<string>();

  // Diagonals starting from the top row (right to left)
  for (var y = height - 1; y >= 0; y--)
  {
    var letters = GetDiagonal(map, 0, y, 1, -1);
    //Console.WriteLine($"1 - {string.Join("", letters)}");
    lines.Add(string.Join("", letters));
  }

  // Diagonals starting from the rightmost column (top to bottom)
  for (var x = 1; x < width; x++)
  {
    var letters = GetDiagonal(map, x, height - 1, 1, -1);
    //Console.WriteLine($"2 - {string.Join("", letters)}");
    lines.Add(string.Join("", letters));
  }

  return lines.ToList();
}

List<char> GetDiagonal(char[,] map, int startRow, int startColumn, int rowStep, int columnStep)
{
  var width = map.GetLength(0);
  var height = map.GetLength(1);
  var letters = new List<char>();

  var x = startRow;
  var y = startColumn;
  while (x >= 0 && x < width && y >= 0 && y < height)
  {
    letters.Add(map[x, y]);
    x += rowStep;
    y += columnStep;
  }

  return letters;
}

int CountLines(IEnumerable<string> lines)
{
  const string pattern = @"(?=(XMAS|SAMX))";

  var count = 0;
  foreach (var line in lines)
  {
    var matches = Regex.Matches(line, pattern);
    foreach (Match match in matches)
    {
      //Console.WriteLine(match.Value);
      count++;
    }
  }

  return count;
}

char[,] BuildMap(string[] lines)
{
  var height = lines.Length;
  var width = lines[0].Length;
  var map = new char[width, height];

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