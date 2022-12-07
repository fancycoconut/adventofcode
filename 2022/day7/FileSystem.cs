using System;
using System.Linq;
using System.Collections.Generic;

namespace Day7;

public class FileSystem
{
  private Stack<FileSystemObject> currentDirectory;

  private FileSystemObject currentFile;
  //private FileSystemObject previousFile;
  private FileSystemObject fileSystemRoot;

  public FileSystem()
  {
    currentDirectory = new();

    fileSystemRoot = new();
    currentFile = fileSystemRoot;
    //previousFile = fileSystemRoot;
    currentDirectory.Push(fileSystemRoot);
  }

  public void Parse(string[] input)
  {
    foreach (var line in input)
    {
      var type = GetOutputType(line);
      HandleOutput(type, line);
    }
  }

  public List<Folder> GetAllFolders()
  {
    var folders = new List<Folder>();
    TraverseFolders(folders, fileSystemRoot);
    return folders;
  }

  public void TraverseFolders(List<Folder> folders, FileSystemObject folder)
  {
    var folderSize = 0;
    foreach (var child in folder.Children)
    {
      if (child.IsFolder)
      {
        TraverseFolders(folders, child);
        folderSize += folders.Last().Size;
        continue;
      }
      folderSize += child.Size;
    }

    folders.Add(new Folder {
      Name = folder.Name,
      Size = folderSize
    });
  }

  private void HandleOutput(OutputType type, string data)
  {
    var parts = data.Split(" ");
    switch (type)
    {
      case OutputType.Command:
        HandleCommands(data);
        break;
      case OutputType.Directory:
        Console.WriteLine($"Found directory: {parts[1]}");

        currentFile.Children.Add(new FileSystemObject {
          Name = parts[1],
          IsFolder = true,
          Parent = currentDirectory.Peek()
        });
        break;
      case OutputType.File:
        Console.WriteLine($"Found file: {parts[1]}");

        TryGetFileParts(parts[1], out var name, out var extension);
        currentFile.Children.Add(new FileSystemObject {
          Name = name,
          Extension = extension,
          IsFolder = false,
          Size = int.Parse(parts[0])
        });
        break;
      default:
        throw new InvalidOperationException("Unsupported output type!");
    }
  }

  private void HandleCommands(string data)
  {
    var parts = data.Split(" ");
    if (parts[1] != "cd") return;

    if (parts[2] == "..")
    {
      var previousDirectory = currentDirectory.Pop();
      currentFile = currentDirectory.Peek();
      //previousFile = currentFile.Parent;

      //Console.WriteLine($"Previous: {previousFile.Name}");
      Console.WriteLine($"Current: {currentFile.Name}");
    }
    else if (parts[2] == "/")
    {
      while (currentDirectory.Any())
      {
        currentDirectory.Pop();
      }

      currentFile = fileSystemRoot;
      currentDirectory.Push(fileSystemRoot);
      Console.WriteLine($"Current Directory is: {currentFile.Name}");
    }
    else
    {
      //Console.WriteLine($"Previous Directory is: {previousFile.Name}");
      Console.WriteLine($"Current Directory is: {parts[2]} {currentFile.Name}");

      var destination = currentFile.Children.First(x => x.Name == parts[2]);
      //previousFile = currentFile;
      currentFile = destination;

      currentDirectory.Push(destination);
    }
  }

  private void TryGetFileParts(string filename, out string name, out string extension)
  {
    var fileParts = filename.Split(".");
    name = fileParts[0];
    try {
      extension = fileParts[1];
    }
    catch {
      extension = "";
    }
  }

  private OutputType GetOutputType(string text)
  {
    if (text.StartsWith("$ ")) return OutputType.Command;
    if (text.StartsWith("dir")) return OutputType.Directory;
    return OutputType.File;
  }
}
