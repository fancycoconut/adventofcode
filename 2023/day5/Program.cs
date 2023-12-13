// See https://aka.ms/new-console-template for more information

using System.Collections.Concurrent;

Console.WriteLine("Hello, World!");

Part1("sample.txt");
Part1("input.txt");
Part2("sample.txt");
Part2("input.txt");

void Part1(string filename)
{
    var lines = File.ReadAllLines(filename);
    var seeds = GetSeeds(lines[0]);

    var seedToSoilMap = BuildLookupMap("seed-to-soil", lines);
    var soilToFertilizerMap = BuildLookupMap("soil-to-fertilizer", lines);
    var fertilizerToWaterMap = BuildLookupMap("fertilizer-to-water", lines);
    var waterToLightMap = BuildLookupMap("water-to-light", lines);
    var lightToTemperatureMap = BuildLookupMap("light-to-temperature", lines);
    var temperatureToHumidityMap = BuildLookupMap("temperature-to-humidity", lines);
    var humidityToLocationMap = BuildLookupMap("humidity-to-location", lines);
    
    // Convert to location
    var locations = new List<ulong>();
    foreach (var seed in seeds)
    {
        var soil = seedToSoilMap.Lookup(seed);
        var fertilizer = soilToFertilizerMap.Lookup(soil);
        var water = fertilizerToWaterMap.Lookup(fertilizer);
        var light = waterToLightMap.Lookup(water);
        var temperature = lightToTemperatureMap.Lookup(light);
        var humidity = temperatureToHumidityMap.Lookup(temperature);
        var location = humidityToLocationMap.Lookup(humidity);
        
        locations.Add(location);
    }

    var lowestLocation = locations.Min();
    Console.WriteLine($"Part 1 - The lowest location for all seeds is: {lowestLocation}");
}

void Part2(string filename)
{
    var lines = File.ReadAllLines(filename);
    var seedsRange = GetSeedRanges(lines[0]);
    
    var seedToSoilMap = BuildLookupMap("seed-to-soil", lines);
    var soilToFertilizerMap = BuildLookupMap("soil-to-fertilizer", lines);
    var fertilizerToWaterMap = BuildLookupMap("fertilizer-to-water", lines);
    var waterToLightMap = BuildLookupMap("water-to-light", lines);
    var lightToTemperatureMap = BuildLookupMap("light-to-temperature", lines);
    var temperatureToHumidityMap = BuildLookupMap("temperature-to-humidity", lines);
    var humidityToLocationMap = BuildLookupMap("humidity-to-location", lines);

    var locations = new ConcurrentBag<ulong>();
    var options = new ParallelOptions
    {
        MaxDegreeOfParallelism = Environment.ProcessorCount
    };
    
    Parallel.ForEach(seedsRange, options, range =>
    {
        for (var seed = range.Item1; seed <= range.Item2; seed++)
        {
            var soil = seedToSoilMap.Lookup(seed);
            var fertilizer = soilToFertilizerMap.Lookup(soil);
            var water = fertilizerToWaterMap.Lookup(fertilizer);
            var light = waterToLightMap.Lookup(water);
            var temperature = lightToTemperatureMap.Lookup(light);
            var humidity = temperatureToHumidityMap.Lookup(temperature);
            var location = humidityToLocationMap.Lookup(humidity);
            
            locations.Add(location);
        }
    });

    var lowestLocation = locations.Min();
    Console.WriteLine($"Part 2 - The lowest location for all seeds is: {lowestLocation}");
}

bool SeedFoundInRange(ulong seed, List<(ulong, ulong)> seedsRange)
{
    foreach (var range in seedsRange)
    {
        if (range.Item1 <= seed && seed <= range.Item2) return true;
    }

    return false;
}

List<ulong> GetSeeds(string seedsLine)
{
    var colonIndex = seedsLine.AsSpan().IndexOf(":") + 2;
    var seeds = seedsLine.AsSpan()[colonIndex..].ToString()
        .Split(" ")
        .Select(x => ulong.Parse(x))
        .ToList();

    return seeds;
}

List<(ulong, ulong)> GetSeedRanges(string seedsLine)
{
    var colonIndex = seedsLine.AsSpan().IndexOf(":") + 2;
    var seeds = seedsLine.AsSpan()[colonIndex..].ToString()
        .Split(" ")
        .Select(x => ulong.Parse(x))
        .ToArray();

    var seedRanges = new List<(ulong, ulong)>();
    var length = seeds.Length / 2;
    for (var i = 0; i < length + 1; i = i + 2)
    {
        var start = seeds[i];
        var range = seeds[i + 1] - 1;
        var end = start + range;
        
        seedRanges.Add((start, end));
    }

    return seedRanges;
}

LookupMap BuildLookupMap(string category, string[] lines)
{
    var categoryMaps = ReadCategoryMaps(category, lines).ToList();

    return new LookupMap(categoryMaps);
}

IEnumerable<CategoryMap> ReadCategoryMaps(string category, string[] lines)
{
    var found = false;
    var expectedSection = $"{category} map:";
    for (var i = 2; i < lines.Length; i++)
    {
        var line = lines[i];
        if (line == expectedSection)
        {
            found = true;
            continue;
        }

        if (line == "" && found)
        {
            yield break;
        }

        if (!found) continue;
        
        var sections = line.Split(" ");
        if (!ulong.TryParse(sections[0], out var destination))
        {
            continue;
        }
        
        var source = ulong.Parse(sections[1]);
        var range = ulong.Parse(sections[2]);

        yield return new CategoryMap(category, source, destination, range);
    }
}

public record CategoryMap(string Name, ulong Source, ulong Destination, ulong Range);

public class LookupMap
{
    private readonly List<CategoryMap> _categoryMaps;

    public LookupMap(List<CategoryMap> categoryMaps)
    {
        _categoryMaps = categoryMaps;
    }

    public ulong Lookup(ulong source)
    {
        foreach (var category in _categoryMaps)
        {
            var lowerSource = category.Source;
            var upperSource = category.Source + category.Range - 1;
            if (lowerSource <= source && source <= upperSource)
            {
                var diff = source - lowerSource;
                var destination = category.Destination + diff;
                return destination;
            }
        }

        return source;
    }

    public ulong ReverseLookup(ulong destination)
    {
        foreach (var category in _categoryMaps)
        {
            ulong lowerDestination = category.Destination;
            var upperDestination = category.Destination + category.Destination - 1;
            if (lowerDestination <= destination && destination <= upperDestination)
            {
                var diff = destination - lowerDestination;
                var source = category.Source + diff;
                return source;
            }
        }

        return destination;
    }

    public CategoryMap GetLowestDestination()
    {
        return _categoryMaps.OrderBy(x => x.Destination).First();
    }

    public CategoryMap GetOverlappingDestinationRange(CategoryMap dest)
    {
        return _categoryMaps.First(src 
            => src.Destination + src.Range <= dest.Source + dest.Range);
    }
}