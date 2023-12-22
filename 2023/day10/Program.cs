// See https://aka.ms/new-console-template for more information

using System.Text;

Console.WriteLine("Hello, World!");

Part1("sample.txt");

void Part1(string filename)
{
    var map = ParseMap(filename);
    var startingPosition = FindStartingPosition(map);
    
    NavigateMap(startingPosition, map);
    
    DumpMap($"{Directory.GetCurrentDirectory()}/input-dump.txt", map);
}

void NavigateMap((int x, int y) startingPoint, char[,] map, int currentStep = 0, Direction cameFromDirection = Direction.Unknown)
{
    // var possibleMoves = new Queue<(int x, int y, int currentStep)>();
    //
    // var start = (startingPoint.x, startingPoint.y, 0);
    // possibleMoves.Enqueue(start);
    // while (possibleMoves.Any())
    // {
    //     var currentPosition = possibleMoves.Dequeue();
    //     
    //     if (CanMoveUp(currentPosition.x, currentPosition.y, map))
    //     {
    //         var upPosition = (startingPoint.x, startingPoint.y - 1, currentPosition.currentStep++);
    //         possibleMoves.Enqueue(upPosition);
    //     }
    //     
    //     if (CanMoveDown(currentPosition.x, currentPosition.y, map))
    //     {
    //         var downPosition = (startingPoint.x, startingPoint.y + 1, currentPosition.currentStep++);
    //         possibleMoves.Enqueue(downPosition);
    //     }
    //     
    //     if (CanMoveLeft(currentPosition.x, currentPosition.y, map))
    //     {
    //         var leftPosition = (startingPoint.x - 1, startingPoint.y, currentPosition.currentStep++);
    //         possibleMoves.Enqueue(leftPosition);
    //     }
    //     
    //     if (CanMoveRight(currentPosition.x, currentPosition.y, map))
    //     {
    //         var rightPosition = (startingPoint.x + 1, startingPoint.y, currentPosition.currentStep++);
    //         possibleMoves.Enqueue(rightPosition);
    //     }
    //     
    //     map[currentPosition.x, currentPosition.y] = currentPosition.currentStep.ToString().First();
    // }

    var currentPosition = startingPoint;
    if (CanMoveUp(currentPosition.x, currentPosition.y, map) && cameFromDirection != Direction.Down)
    {
        var upPosition = (currentPosition.x, currentPosition.y - 1);
        NavigateMap(upPosition, map, currentStep++, Direction.Down);
    }
    
    if (CanMoveDown(currentPosition.x, currentPosition.y, map) && cameFromDirection != Direction.Up)
    {
        var downPosition = (currentPosition.x, currentPosition.y + 1);
        NavigateMap(downPosition, map, currentStep++, Direction.Up);
    }
    
    if (CanMoveLeft(currentPosition.x, currentPosition.y, map) && cameFromDirection != Direction.Right)
    {
        var leftPosition = (currentPosition.x - 1, currentPosition.y);
        NavigateMap(leftPosition, map, currentStep++, Direction.Right);
    }
    
    if (CanMoveRight(currentPosition.x, currentPosition.y, map) && cameFromDirection != Direction.Left)
    {
        var rightPosition = (currentPosition.x + 1, currentPosition.y);
        NavigateMap(rightPosition, map, currentStep++, Direction.Left);
    }
    
    map[currentPosition.x, currentPosition.y] = currentStep.ToString().First();
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