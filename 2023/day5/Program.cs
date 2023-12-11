// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Part1("sample.txt");
Part1("input.txt");

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
    Console.WriteLine($"The lowest location for all seeds is: {lowestLocation}");
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

LookupMap BuildLookupMap(string category, string[] lines)
{
    var categoryMaps = ReadCategoryMaps(category, lines).ToList();

    return new LookupMap(categoryMaps);
}

SuperDictionary BuildMap(string category, string[] lines)
{
    var output = new Dictionary<ulong, ulong>();
    var categoryMaps = ReadCategoryMaps(category, lines).ToList();
    foreach (var map in categoryMaps)
    {
        for (ulong i = 0; i < map.Range; i++)
        {
            var source = map.Source + i;
            var destination = map.Destination + i;
            //Console.WriteLine($"{category} - src: {source} : dest: {destination}, range: {map.Range}");
            output[source] = destination;
        }
    }

    return new SuperDictionary(output);
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

public class SuperDictionary : Dictionary<ulong, ulong>
{
    private readonly Dictionary<ulong, ulong> _map;

    public SuperDictionary(Dictionary<ulong, ulong> map)
    {
        _map = map;
    }

    public ulong Lookup(ulong source)
    {
        return _map.TryGetValue(source, out var destination) 
            ? destination 
            : source;
    }
}

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
}