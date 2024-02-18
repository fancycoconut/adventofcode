
Console.WriteLine("Hello, World!");

Part1("sample.txt");
Part1("sample2.txt");
Part1("input.txt");

Part2("part2-sample1.txt");

void Part1(string filename)
{
    var map = ParseMap(filename);
    var startingPosition = FindStartingPosition(map);

    var visitedLocations = new HashSet<(int, int)>();
    BreadthFirstSearch(startingPosition, map, visitedLocations);

    var farthestNode = visitedLocations.Count;
    Console.WriteLine($"Part 1 - The furtherst node is: {farthestNode / 2}");
}

void Part2(string filename)
{
    var map = ParseMap(filename);
    var startingPosition = FindStartingPosition(map);
    var startingPipe = DeriveStartPositionPipe(startingPosition, map);
    map[startingPosition.Item1, startingPosition.Item2] = startingPipe;

    var visitedLocations = new HashSet<(int, int)>();
    BreadthFirstSearch(startingPosition, map, visitedLocations);

    var enclosedTiles = CalculateEnclosedTiles(map, visitedLocations);

    Console.WriteLine($"Part 2 - Number of enclosed tiles: {enclosedTiles}");
}

char DeriveStartPositionPipe((int x, int y) startPosition, char[,] map)
{
    var width = map.GetLength(1);
    var height = map.GetLength(0);

    var topPosition = (startPosition.x, startPosition.y - 1 < 0 ? 0 : startPosition.y - 1);
    var bottomPosition = (startPosition.x, startPosition.y + 1 >= height ? height - 1 : startPosition.y + 1);
    var leftPosition = (startPosition.x - 1 < 0 ? 0 : startPosition.x - 1, startPosition.y);
    var rightPosition = (startPosition.x + 1 >= width ? width - 1 : startPosition.x + 1, startPosition.y);

    if ("-J7".Contains(map[rightPosition.Item1, rightPosition.y]) && "|JL".Contains(map[bottomPosition.Item1, bottomPosition.Item2]))
    {
        return 'F';
    }

    if ("-LF".Contains(map[leftPosition.Item1, leftPosition.y]) && "|JL".Contains(map[bottomPosition.Item1, bottomPosition.Item2]))
    {
        return '7';
    }

    if ("|7F".Contains(map[topPosition.Item1, topPosition.Item2]) && "7-J".Contains(map[rightPosition.Item1, rightPosition.y]))
    {
        return 'L';
    }

    if ("|7F".Contains(map[topPosition.Item1, topPosition.Item2]) && "F-L".Contains(map[leftPosition.Item1, leftPosition.y]))
    {
        return 'J';
    }

    throw new InvalidOperationException("Unable to derive start position");
}

int CalculateEnclosedTiles(char[,] map, HashSet<(int, int)> visitedLocations)
{
    var numOfEnclosedTiles = 0;
    var width = map.GetLength(0);
    var height = map.GetLength(1);

    for (var y = 0; y < height; y++)
    {
        for (var x = 0; x < width; x++)
        {
            var currentPosition = (x, y);
            var numOfIntersections = RayCastingNumberOfIntersectionsFromLeft(currentPosition, map, visitedLocations);

            if (numOfIntersections % 2 != 0)
            {
                numOfEnclosedTiles++;
            }
        }
    }

    return numOfEnclosedTiles;
}

// Ray casting algorithm
// https://en.wikipedia.org/wiki/Point_in_polygon
// One simple way of finding whether the point is inside or outside a simple polygon is to test how many times a ray,
// starting from the point and going in any fixed direction, intersects the edges of the polygon. If the point is on
// the outside of the polygon the ray will intersect its edge an even number of times. If the point is on the inside
// of the polygon then it will intersect the edge an odd number of times.
int RayCastingNumberOfIntersectionsFromLeft((int, int) currentPosition, char[,] map, HashSet<(int, int)> visitedLocations)
{
    // We can just count all the vertical pipes and corners in a straight line
    var numberOfEdges = 0;
    var width = map.GetLength(0);

    var currentTile = map[currentPosition.Item1, currentPosition.Item2];
    if (currentTile != '.') return 0;

    for (var x = currentPosition.Item1; x < width; x++)
    {
        var tile = map[x, currentPosition.Item2];
        var pipeIsAVistedLocation = visitedLocations.Contains((x, currentPosition.Item2));

        if ("IFL".Contains(tile) && pipeIsAVistedLocation)
        {
            numberOfEdges++;
        }
    }

    return numberOfEdges;
}

void BreadthFirstSearch((int x, int y) startPosition, char[,] map, HashSet<(int, int)> visitedNodes)
{
    var queue = new Queue<(int, int)>();

    queue.Enqueue(startPosition);
    visitedNodes.Add(startPosition);

    while (queue.Any())
    {
        var currentPosition = queue.Dequeue();

        foreach (var edge in GetAdjacentEdges(currentPosition, map))
        {
            if (visitedNodes.Contains(edge)) continue;
            visitedNodes.Add(edge);
            queue.Enqueue(edge);
        }
    }
}

List<(int, int)> GetAdjacentEdges((int x, int y) currentPosition, char[,] map)
{
    var adjacentEdges = new List<(int, int)>();

    var x = currentPosition.x;
    var y = currentPosition.y;
    if (CanMoveUp(x, y, map))
    {
        adjacentEdges.Add((x, y - 1));
    }

    if (CanMoveDown(x, y, map))
    {
        adjacentEdges.Add((x, y + 1));
    }

    if (CanMoveLeft(x, y, map))
    {
        adjacentEdges.Add((x - 1, y));
    }

    if (CanMoveRight(x, y, map))
    {
        adjacentEdges.Add((x + 1, y));
    }

    return adjacentEdges;
}

bool CanMoveUp(int x, int y, char[,] map)
{
    if (y - 1 < 0) return false;

    var current = map[x, y];
    var target = map[x, y - 1];

    return (current, target) switch
    {
        ('|', 'F') => true,
        ('|', '|') => true,
        ('|', '7') => true,
        ('L', 'F') => true,
        ('L', '|') => true,
        ('L', '7') => true,
        ('J', 'F') => true,
        ('J', '|') => true,
        ('J', '7') => true,
        ('S', 'F') => true,
        ('S', '|') => true,
        ('S', '7') => true,
        _ => false
    };
}

bool CanMoveDown(int x, int y, char[,] map)
{
    var height = map.GetLength(1);
    if (y + 1 >= height) return false;

    var current = map[x, y];
    var target = map[x, y + 1];

    return (current, target) switch
    {
        ('|', 'L') => true,
        ('|', '|') => true,
        ('|', 'J') => true,
        ('F', 'L') => true,
        ('F', '|') => true,
        ('F', 'J') => true,
        ('7', 'L') => true,
        ('7', '|') => true,
        ('7', 'J') => true,
        ('S', 'L') => true,
        ('S', '|') => true,
        ('S', 'J') => true,
        _ => false
    };
}

bool CanMoveLeft(int x, int y, char[,] map)
{
    if (x - 1 < 0) return false;

    var current = map[x, y];
    var target = map[x - 1, y];

    return (current, target) switch
    {
        ('-', 'F') => true,
        ('-', '-') => true,
        ('-', 'L') => true,
        ('7', 'F') => true,
        ('7', '-') => true,
        ('7', 'L') => true,
        ('J', 'F') => true,
        ('J', '-') => true,
        ('J', 'L') => true,
        ('S', 'F') => true,
        ('S', '-') => true,
        ('S', 'L') => true,
        _ => false
    };
}

bool CanMoveRight(int x, int y, char[,] map)
{
    var width = map.GetLength(0);
    if (x + 1 >= width) return false;

    var current = map[x, y];
    var target = map[x + 1, y];

    return (current, target) switch
    {
        ('-', '7') => true,
        ('-', '-') => true,
        ('-', 'J') => true,
        ('F', '7') => true,
        ('F', '-') => true,
        ('F', 'J') => true,
        ('L', '7') => true,
        ('L', '-') => true,
        ('L', 'J') => true,
        ('S', '7') => true,
        ('S', '-') => true,
        ('S', 'J') => true,
        _ => false
    };
}

(int, int) FindStartingPosition(char[,] map)
{
    var width = map.GetLength(0);
    var height = map.GetLength(1);

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