namespace Day7;

public class Program
{
  public static void Main(string[] args)
  {
    Console.WriteLine("Hello World!");

    var fs = new FileSystem();
    var terminalOutput = File.ReadAllLines("input.txt");
    fs.Parse(terminalOutput);

    var folders = fs.GetAllFolders();
    Console.WriteLine(folders.Count);
    var totalSize = folders.Where(x => x.Size <= 100000)
      .Select(x => x.Size)
      .Sum();
    
    Console.WriteLine(totalSize);
  }
}