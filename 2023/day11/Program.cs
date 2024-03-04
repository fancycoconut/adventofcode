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
    var galaxies = FindGalaxies(aoc, expansionFactor, columnsWithoutGalaxies, rowsWithoutGalaxies).ToList();
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

IEnumerable<(int, int)> FindGalaxies(Puzzle aoc, int expansionFactor, HashSet<int> columnsWithoutGalaxies, HashSet<int> rowsWithoutGalaxies)
{
    var map = aoc.Map;

    var i = 1;
    for (var y = 0; y < aoc.Height; y++)
    {
        for (var x = 0; x < aoc.Width; x++)
        {
            var spot = map[x, y];
            if (spot == '#')
            {
                var targetX = x;
                var targetY = y;
                if (columnsWithoutGalaxies.Any(z => z >= x))
                {
                    targetX += expansionFactor;
                }

                if (rowsWithoutGalaxies.Any(z => z >= y))
                {
                    targetY += expansionFactor;
                }

                yield return (targetX, targetY);
                i++;
            }
        }
    }
}