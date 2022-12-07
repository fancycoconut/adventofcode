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
    var totalSize = folders.Where(x => x.Size <= 100000)
      .Select(x => x.Size)
      .Sum();
    
    Console.WriteLine(totalSize);

    var totalDiskSpace = 70000000;
    var requiredDiskSpace = 30000000;

    var rootDirectory = folders.Single(x => x.Name == null);
    var remainingDiskSpace = totalDiskSpace - rootDirectory.Size;
    Console.WriteLine($"Remaining disk space: {remainingDiskSpace}");

    var minimumFileSizeToDelete = requiredDiskSpace - remainingDiskSpace;
    Console.WriteLine($"Minimum file size to delete: {minimumFileSizeToDelete}");

    var folderToDelete = folders.Where(x => x.Size > minimumFileSizeToDelete)
      .ToList();

    var smallest = folderToDelete.First();
    var min = smallest.Size;
    foreach (var folder in folderToDelete)
    {
      if (folder.Size < min)
      {
        min = folder.Size;
        smallest = folder;
      }
    }

    Console.WriteLine($"Delete the folder: {smallest.Name} {smallest.Size}");
  }
}