// See https://aka.ms/new-console-template for more information
using AdventOfCode;

Console.WriteLine("Hello, World!");

Part1("sample.txt");

void Part1(string filename)
{
    var aoc = new Puzzle(filename);
    var columnsWithoutGalaxies = GetColumnsWithoutGalaxies(aoc).ToHashSet();
    var rowsWithoutGalaxies = GetRowsWithoutGalaxies(aoc).ToHashSet();

    var expansionFactor = 2;
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

IEnumerable<(int, int)> ApplyExpansionFactor(int expansionFactor, List<(int col, int row)> galaxyLocations, HashSet<int> columnsWithoutGalaxies, HashSet<int> rowsWithoutGalaxies)
{
    foreach (var location in galaxyLocations)
    {
        var c = location.col;
        var r = location.row;

        var i = 0;
        foreach (var column in columnsWithoutGalaxies)
        {
            var expandedCol = column + expansionFactor - location.col;
            i += expandedCol;
            if (location.col > column)
            {
                c = location.col + i;
                break;
            }
        }

        i = 0;
        foreach (var row in rowsWithoutGalaxies)
        {
            var expandedRow = row + expansionFactor - location.row;
            i += expandedRow;
            if (location.row > row)
            {
                r = location.row + i;
                break;
            }
        }

        yield return (c, r);
    }
}