using System.Text;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Part1("sample.txt");
Part1("input.txt");

// 00992111777.44.333....5555.6666.....8888..
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

  var expandedDiskMap = ExpandDiskMapV2(diskmap);
  PrintExpandedDiskMap(expandedDiskMap);
  var updatedDiskMap = RearrangeFileBlocksV2(expandedDiskMap);
  Console.WriteLine("Disk map after re-arrangement:");
  PrintExpandedDiskMap(updatedDiskMap);
  //Console.WriteLine($"Diskmap state after re-arrangement: {updatedDiskMap}");
 
  //var checksum = CalculateChecksum(updatedDiskMap);
  //Console.WriteLine($"Part 2 - Checksum: {checksum}");
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

List<RawDataBlock> RearrangeFileBlocksV2(List<RawDataBlock> expandedDiskMap)
{
  var input = expandedDiskMap;

  var left = 0;
  var right = input.Count - 1;
  var id = expandedDiskMap.Max(x => x.Id); 

  while (left != right && left < input.Count && right > 0)
  {
    var leftValue = input[left];
    var rightValue = input[right];
    if (leftValue.IsFreeSpace == false) {
      left++;
      continue;
    }
    if (rightValue.IsFreeSpace) {
      right--;
      continue;
    }

    // Swap - moving back to front)
    if (leftValue.BlockSize >= rightValue.BlockSize && id == rightValue.Id) {
      var remainingFreeSpace = leftValue.BlockSize - rightValue.BlockSize;
      if (remainingFreeSpace > 0) {
        var temp = leftValue;
        input[left] = input[right];
        input.Insert(left + 1, new RawDataBlock {
          Id = -1,
          BlockSize = remainingFreeSpace,
          IsFreeSpace = true
        });
        input[right] = temp;
        
      }
      else
      {
        var temp = leftValue;
        input[left] = input[right];
        input[right] = temp;
      }

      left = 0;
      id--;
    }
    else
    {
      left++;
    }

    //PrintExpandedDiskMap(input);
  }

  return input;
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

void PrintExpandedDiskMap(List<RawDataBlock> expandedDiskMap)
{
  var sb = new StringBuilder();
  foreach (var dataBlock in expandedDiskMap)
  {
    if (dataBlock.IsFreeSpace == false)
    {
      for (var i = 0; i < dataBlock.BlockSize; i++)
      {
        sb.Append(dataBlock.Id);
      }
      continue;
    }

    if (dataBlock.IsFreeSpace)
    {
      for (var i = 0; i < dataBlock.BlockSize; i++)
      {
        sb.Append(".");
      }
      continue;
    }
  }

  Console.WriteLine(sb.ToString());
}

List<RawDataBlock> ExpandDiskMapV2(string diskmap)
{
  double id = 0;
  var expandedDiskMap = new List<RawDataBlock>();

  var input = diskmap.AsSpan();
  for (var i = 0; i < input.Length; i++)
  {
    var blockSize = int.Parse(input[i].ToString());
    var isFreeSpace = i % 2 == 1;

    RawDataBlock? dataBlock;
    if (!isFreeSpace)
    {
      dataBlock = new RawDataBlock {
        Id = id,
        BlockSize = blockSize,
        IsFreeSpace = false
      };
      id++;
    }
    else
    {
      dataBlock = new RawDataBlock {
        Id = -1,
        BlockSize = blockSize,
        IsFreeSpace = true
      };
    }

    expandedDiskMap.Add(dataBlock);
  }

  return expandedDiskMap;
}

public class RawDataBlock
{
  public double Id { get; set; }
  public int BlockSize { get; set; }
  public bool IsFreeSpace { get; set; }
}
