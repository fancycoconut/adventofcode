// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using System.ComponentModel;

Console.WriteLine("Hello, World!");

Part1("sample.txt", new GameCubesConfiguration {
  Red = 12,
  Green = 13,
  Blue = 14
});
Part1("input.txt", new GameCubesConfiguration {
  Red = 12,
  Green = 13,
  Blue = 14
});

void Part1(string filename, GameCubesConfiguration configuration)
{
  var lines = File.ReadAllLines(filename);

  var sum = 0;
  foreach (var line in lines)
  {
    var gameId = GetGameId(line);
    //Console.WriteLine($"Game: {gameId}");

    var drawnCubeSet = ParseDrawnCubes(line)
      .ToList();

    if (CheckGamePossibility(drawnCubeSet, configuration))
    {
      sum += gameId;
    }
  }
  
  Console.WriteLine($"Game id sum for {filename}: {sum}");
}

bool CheckGamePossibility(List<DrawnCubeSet> drawnResults, GameCubesConfiguration configuration)
{
  foreach (var result in drawnResults)
  {
    var redsAreInLimit = result.Red <= configuration.Red;
    var greensAreInLimit = result.Green <= configuration.Green;
    var bluesAreInLimit = result.Blue <= configuration.Blue;

    if (!redsAreInLimit || !greensAreInLimit || !bluesAreInLimit) return false;
  }

  return true;
}

IEnumerable<DrawnCubeSet> ParseDrawnCubes(string game)
{
  var colonPosition = game.IndexOf(":", StringComparison.Ordinal) + 2;
  var raw = game[colonPosition..];
  var sets = raw.Split("; ");

  foreach (var set in sets)
  {
    yield return GetDrawnCubesFromResult(set);
  }
}

DrawnCubeSet GetDrawnCubesFromResult(string result)
{
  var pairs = result.Split(", ")
    .Select(x => x.Split(" "));

  var red = 0;
  var green = 0;
  var blue = 0;
  foreach (var pair in pairs)
  {
    switch (pair[1])
    {
      case "red":
        red = int.Parse(pair[0]);
        break;
      case "green":
        green = int.Parse(pair[0]);
        break;
      case "blue":
        blue = int.Parse(pair[0]);
        break;
    }
  }

  return new DrawnCubeSet(red, green, blue);
}

int GetGameId(string line)
{
  var colonPosition = line.IndexOf(":");
  var gameId = line.AsSpan(5, colonPosition - 5);
  return int.Parse(gameId);
}

public class GameCubesConfiguration
{
  public int Red { get; set; }
  public int Green { get; set; }
  public int Blue { get; set; }
}

public record DrawnCubeSet(int Red, int Green, int Blue);