namespace Day12;

public record Coordinate
{
  public int X { get; set; }
  public int Y { get; set; }
  public Coordinate? Last { get; set; }

  public Coordinate(int x, int y)
  {
    X = x;
    Y = y;
  }

  public Coordinate Up() => new Coordinate(X, Y-1);
  public Coordinate Down() => new Coordinate(X, Y+1);
  public Coordinate Left() => new Coordinate(X-1, Y);
  public Coordinate Right() => new Coordinate(X+1, Y);

  public IEnumerable<Coordinate> GetAllPaths()
  {
    var coordinate = Last;
    while (coordinate != null)
    {
      yield return coordinate;
      coordinate = coordinate.Last;
    }
  }
}