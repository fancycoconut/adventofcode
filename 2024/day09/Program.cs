using System.Text;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Part1("sample.txt");
Part1("input.txt");

Part2("sample.txt");

void Part1(string filename)
{
  var diskmap = File.ReadAllText(filename);

  var expandedDiskMap = ExpandDiskmap(diskmap);
  //Console.WriteLine($"Diskmap expanded: {expandedDiskMap}");
  var updatedDiskMap = RearrangeFileblocks(expandedDiskMap);
  //Console.WriteLine($"Diskmap state after re-arrangement: {updatedDiskMap}");
 
  var checksum = CalculateChecksum(updatedDiskMap);
  Console.WriteLine($"Part 1 - Checksum: {checksum}");
}

void Part2(string filename)
{
  var diskmap = File.ReadAllText(filename);

  var expandedDiskMap = ExpandDiskmapV2(diskmap);

  Console.WriteLine($"Diskmap expanded: {string.Join("", expandedDiskMap)}");
  var updatedDiskMap = RearrangeFileBlocksV2(expandedDiskMap);
  Console.WriteLine($"Diskmap state after re-arrangement: {string.Join("", expandedDiskMap)}");
 
  var checksum = CalculateChecksum(updatedDiskMap);
  Console.WriteLine($"Part 2 - Checksum: {checksum}");
}

double CalculateChecksum(List<string> expandedDiskMap)
{
  double total = 0;
  var input = expandedDiskMap.ToArray();

  for (var i = 0; i < input.Length; i++)
  {
    var current = input[i];
    if (current == ".") continue;

    var id = double.Parse(current.ToString());
    total += i * id;
  }

  return total;
}

List<string> RearrangeFileBlocksV2(List<string> expandedDiskMap)
{
  var input = expandedDiskMap.ToArray();

  var left = 0;
  var right = input.Length - 1;
  var id = double.Parse(expandedDiskMap.Last());

  while (left != right && left < input.Length && right > 0)
  {
    var leftValue = input[left];
    var rightValue = input[right];
    double.TryParse(rightValue, out double rightValueAsId);

    if (leftValue != ".") {
      left++;
      continue;
    }
    if (rightValue == ".") {
      right--;
      continue;
    }

    // Swap - moving back to front
    if (id == rightValueAsId)
    {
      // Check if enough space in front
      var rightBlockSize = 0;

      var tempRight = right;
      var tempRightValue = rightValue;
      while (tempRightValue == rightValue && tempRight > 0)
      {
        rightBlockSize++;
        tempRight--;
        tempRightValue = input[tempRight];
      }

      var leftBlockSize = 0;
      var tempLeft = left;
      var tempLeftValue = leftValue;
      while (tempLeftValue == leftValue && tempLeft < input.Length)
      {
        leftBlockSize++;
        tempLeft++;
        tempLeftValue = input[tempLeft];
      }

      // Swap only if there are enough space
      if (leftBlockSize >= rightBlockSize)
      {
        var originalLeftValue = leftValue;
        for (var x = left; x < left + rightBlockSize; x++)
        {
          input[x] = rightValue;
        }

        for (var y = right; y >= right - rightBlockSize; y--)
        {
          input[y] = originalLeftValue;
        }
      }

      id--;
    }
    else
    {
      left++;
      right--;
    }
  }

  return input.ToList();
}

// Using 2 pointer approach
List<string> RearrangeFileblocks(List<string> expandedDiskMap)
{
  var input = expandedDiskMap.ToArray();

  var left = 0;
  var right = input.Length - 1;

  while (left != right && left < input.Length && right > 0)
  {
    var leftValue = input[left];
    var rightValue = input[right];
    if (leftValue != ".") {
      left++;
      continue;
    }
    if (rightValue == ".") {
      right--;
      continue;
    }

    // Swap - moving back to front
    var temp = input[left];
    input[left] = input[right];
    input[right] = temp;
  }

  return input.ToList();
}

List<string> ExpandDiskmapV2(string diskmap)
{
  double id = 0;
  var expandedDiskMap = new List<string>();

  var input = diskmap.AsSpan();
  for (var i = 0; i < input.Length; i += 2)
  {
    var blockSize = int.Parse(input[i].ToString());
    var freeSpace = i + 1 == input.Length
     ? 0
     : int.Parse(input[i+1].ToString());

    //Console.WriteLine($"Block size and free space: {blockSize} {freeSpace}");

    for (var x = 0; x < blockSize; x++)
    {
      expandedDiskMap.Add(id.ToString());
    }

    for (var y = 0; y < freeSpace; y++)
    {
      expandedDiskMap.Add(".");
    }

    id++;
  }

  return expandedDiskMap;
}

List<string> ExpandDiskmap(string diskmap)
{
  double id = 0;
  var expandedDiskMap = new List<string>();

  var input = diskmap.AsSpan();
  for (var i = 0; i < input.Length; i += 2)
  {
    var blockSize = int.Parse(input[i].ToString());
    var freeSpace = i + 1 == input.Length
     ? 0
     : int.Parse(input[i+1].ToString());

    //Console.WriteLine($"Block size and free space: {blockSize} {freeSpace}");

    for (var x = 0; x < blockSize; x++)
    {
      expandedDiskMap.Add(id.ToString());
    }

    for (var y = 0; y < freeSpace; y++)
    {
      expandedDiskMap.Add(".");
    }

    id++;
  }

  return expandedDiskMap;
}

public record DiskMapFileData(double Id, int BlockSize, int FreeSpace);
