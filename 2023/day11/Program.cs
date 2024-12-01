// See https://aka.ms/new-console-template for more information
using AdventOfCode;

Console.WriteLine("Hello, World!");

Part1("sample.txt");

void Part1(string filename)
{
    var aoc = new Puzzle(filename);
    var columnsWithoutGalaxies = GetColumnsWithoutGalaxies(aoc).ToHashSet();
    var rowsWithoutGalaxies = GetRowsWithoutGalaxies(aoc).ToHashSet();

    var galaxiesBeforeExpansion = FindGalaxies(aoc).ToList();
    
    // 3,0 => 4,0
    // 7,1 => 10,1
    var galaxiesAfterExpansion = ApplyExpansionFactor(expansionFactor, galaxiesBeforeExpansion, columnsWithoutGalaxies, rowsWithoutGalaxies).ToList();
}

IEnumerable<int> GetColumnsWithoutGalaxies(Puzzle aoc)
{
    var map = aoc.Map;
    for (var x = 0; x < aoc.Width; x++)
    {
        var hasGalaxy = false;
        for (var y = 0; y < aoc.Height; y++)
        {
            var spot = map[x, y];
            if (spot == '#')
            {
                hasGalaxy = true;
                break;
            }
        }

        if (!hasGalaxy)
        {
            yield return x;
        }
    }
}

IEnumerable<int> GetRowsWithoutGalaxies(Puzzle aoc)
{
    var map = aoc.Map;
    for (var y = 0; y < aoc.Height; y++)
    {
        var hasGalaxy = false;
        for (var x = 0; x < aoc.Height; x++)
        {
            var spot = map[x, y];
            if (spot == '#')
            {
                hasGalaxy = true;
                break;
            }
        }

        if (!hasGalaxy)
        {
            yield return y;
        }
    }
}

IEnumerable<(int, int)> FindGalaxies(Puzzle aoc)
{
    var map = aoc.Map;
    
    for (var y = 0; y < aoc.Height; y++)
    {
        for (var x = 0; x < aoc.Width; x++)
        {
            var spot = map[x, y];
            if (spot == '#')
            {
                yield return (x, y);
            }
        }
    }
}

IEnumerable<(int, int)> ApplyExpansionFactor(List<(int col, int row)> galaxyLocations, HashSet<int> columnsWithoutGalaxies, HashSet<int> rowsWithoutGalaxies, int width, int height)
{
    var newWidth = width + columnsWithoutGalaxies.Count;
    var newHeight = height + rowsWithoutGalaxies.Count;
    var expandedMap = new char[newWidth, newHeight];

    for (var x = 0; x < newWidth; x++)
    {
        for (var y = 0; y < newHeight; y++)
        {

        }
    }
}

char[,] ApplyExpansionFactor(List<(int col, int row)> galaxyLocations, HashSet<int> columnsWithoutGalaxies, HashSet<int> rowsWithoutGalaxies, int width, int height)
{
    var newWidth = width + columnsWithoutGalaxies.Count;
    var newHeight = height + rowsWithoutGalaxies.Count;
    var expandedMap = new char[newWidth, newHeight];

    for (var x = 0; x < newWidth; x++)
    {
        for (var y = 0; y < newHeight; y++)
        {

        }
    }

    return expandedMap;
}