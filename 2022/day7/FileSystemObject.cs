using System;
using System.Collections.Generic;

namespace Day7;

public class FileSystemObject
{
  public string Name { get; set; } = null!;

  public string Extension { get; set; } = null!;

  public bool IsFolder { get; set; }

  public int Size { get; set; }

  public FileSystemObject Parent { get; set; }
  public List<FileSystemObject> Children { get; set; } = new();
}