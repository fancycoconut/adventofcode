using System.Text;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Part1("sample.txt");
Part1("input.txt");

void Part1(string filename)
{
  var diskmap = File.ReadAllText(filename);

  var expandedDiskMap = ExpandDiskmap(diskmap);
  //Console.WriteLine($"Diskmap expanded: {expandedDiskMap}");
  var updatedDiskMap = RearrangeFileblocks(expandedDiskMap);
  //Console.WriteLine($"Diskmap state after re-arrangement: {updatedDiskMap}");

  //File.WriteAllText("test.txt", updatedDiskMap);

  var checksum = CalculateChecksum(updatedDiskMap);
  Console.WriteLine($"Part 1 - Checksum: {checksum}");
}

double CalculateChecksum(string expandedDiskMap)
{
  double total = 0;
  var input = expandedDiskMap.AsSpan();

  for (var i = 0; i < input.Length; i++)
  {
    var current = input[i];
    if (current == '.') continue;

    var id = double.Parse(current.ToString());
    total += i * id;
  }

  return total;
}

// Using 2 pointer approach
string RearrangeFileblocks(string expandedDiskMap)
{
  var input = expandedDiskMap.ToCharArray();

  var left = 0;
  var right = input.Length - 1;

  while (left != right && left < input.Length && right > 0)
  {
    var leftValue = input[left];
    var rightValue = input[right];
    if (leftValue != '.') {
      left++;
      continue;
    }
    if (rightValue == '.') {
      right--;
      continue;
    }

    // Swap - moving back to front
    if (leftValue == '.' && rightValue != '.')
    {
      var temp = input[left];
      input[left] = input[right];
      input[right] = temp;
    }
  }

  return new string(input);
}

string ExpandDiskmap(string diskmap)
{
  double id = 0;
  var sb = new StringBuilder();

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
      sb.Append(id);
    }

    for (var y = 0; y < freeSpace; y++)
    {
      sb.Append(".");
    }

    id++;
  }

  return sb.ToString();
}