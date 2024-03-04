namespace AdventOfCode;

public class Puzzle
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    public char[,] Map { get; private set; } = null!;

    public Puzzle(string filename)
    {
        ParseMap(filename);
    }

    public void ParseMap(string filename)
    {
        var lines = File.ReadAllLines(filename);

        Height = lines.Length;
        Width = lines[0].Length;
        var map = new char[Width, Height];

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

        Map = map;
    }
}
