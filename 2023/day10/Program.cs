// See https://aka.ms/new-console-template for more information

using System.Text;

Console.WriteLine("Hello, World!");

Part1("sample.txt");

void Part1(string filename)
{
    var map = ParseMap(filename);
    var startingPosition = FindStartingPosition(map);
    var stepTracker = new Dictionary<(int x, int y), int>();


    NavigateMap(startingPosition, map, stepTracker);
    
    DumpMap($"{Directory.GetCurrentDirectory()}/input-dump.txt", map);
}

void NavigateMap((int x, int y) startingPoint, char[,] map, Dictionary<(int x, int y), int> stepTracker, int currentStep = 0, Direction cameFromDirection = Direction.Unknown)
{
    var x = startingPoint.x;
    var y = startingPoint.y;
    if (stepTracker.ContainsKey(startingPoint)) return;

    if (CanMoveUp(x, y, map) && cameFromDirection != Direction.Up)
    {
        var upPosition = (x, y - 1);
        NavigateMap(upPosition, map, stepTracker, currentStep + 1, Direction.Down);
    }

    if (CanMoveDown(x, y, map) && cameFromDirection != Direction.Down)
    {
        var downPosition = (x, y + 1);
        NavigateMap(downPosition, map, stepTracker, currentStep + 1, Direction.Up);
    }

    if (CanMoveLeft(x, y, map) && cameFromDirection != Direction.Left)
    {
        var leftPosition = (x - 1, y);
        NavigateMap(leftPosition, map, stepTracker, currentStep + 1, Direction.Right);
    }

    if (CanMoveRight(x, y, map) && cameFromDirection != Direction.Right)
    {
        var rightPosition = (x + 1, y);
        NavigateMap(rightPosition, map, stepTracker, currentStep + 1, Direction.Left);
    }

    if (stepTracker.ContainsKey(startingPoint))
        stepTracker[startingPoint] += 1;
    else
        stepTracker[startingPoint] = 1;

    map[x, y] = currentStep.ToString().First();
}

void DumpMap(string filename, char[,] map)
{
    var height = map.GetLength(0);
    var width = map.GetLength(1);

    
    var sb = new StringBuilder();
    using var writer = new StreamWriter(filename);

    for (var y = 0; y < height; y++)
    {
        sb.Clear();
        for (var x = 0; x < width; x++)
        {
            sb.Append(map[x, y]);
        }
        
        writer.WriteLine(sb.ToString());
    }
}

bool CanMoveUp(int x, int y, char[,] map)
{
    if (y - 1 < 0) return false;

    var tile = map[x, y - 1];
    return tile switch
    {
        '|' => true,
        'F' => true,
        '7' => true,
        _ => false
    };
}

bool CanMoveDown(int x, int y, char[,] map)
{
    var height = map.GetLength(0);
    if (y + 1 > height) return false;
    
    var tile = map[x, y + 1];
    return tile switch
    {
        '|' => true,
        'J' => true,
        'L' => true,
        _ => false
    };
}

bool CanMoveLeft(int x, int y, char[,] map)
{
    if (x - 1 < 0) return false;
    
    var tile = map[x - 1, y];
    return tile switch
    {
        '-' => true,
        'L' => true,
        'F' => true,
        _ => false
    };
}

bool CanMoveRight(int x, int y, char[,] map)
{
    var width = map.GetLength(1);
    if (x + 1 > width) return false;
    
    var tile = map[x + 1, y];
    return tile switch
    {
        '-' => true,
        'J' => true,
        '7' => true,
        _ => false
    };
}

(int, int) FindStartingPosition(char[,] map)
{
    var height = map.GetLength(0);
    var width = map.GetLength(1);

    for (var y = 0; y < height; y++)
    {
        for (var x = 0; x < width; x++)
        {
            var value = map[x, y];
            if (value == 'S') return (x, y);
        }
    }

    throw new Exception("No starting point found...");
}

char[,] ParseMap(string filename)
{
    var lines = File.ReadAllLines(filename);

    var height = lines.Length;
    var width = lines[0].Length;
    var map = new char[width, height];

    var y = 0;
    var x = 0;
    foreach (var line in lines)
    {
        foreach (var c in line)
        {
            map[x, y] = c;
            x++;
        }
        y++;
        x = 0;
    }
    
    return map;
}

public enum Direction
{
    Unknown,
    Up,
    Down,
    Left,
    Right
}