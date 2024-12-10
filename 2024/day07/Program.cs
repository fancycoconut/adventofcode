// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Part1("sample.txt");
//Part1("input.txt");

Part2("sample.txt");
Part2("input.txt");

void Part1(string filename)
{
  var lines = File.ReadAllLines(filename);

  ulong total = 0;
  foreach (var line in lines)
  {
    var colonIndex = line.IndexOf(':');
    var testValue = ulong.Parse(line[0..colonIndex]);
    var numbers = line[(colonIndex + 1)..].Split(' ')
      .Where(x => x != "")
      .Select(x => ulong.Parse(x))
      .ToArray();

    var isValid = CanSolve(numbers[0], 0, testValue, numbers);
    total += isValid ? testValue : 0;

    Console.WriteLine($"{testValue} - is valid: {isValid}");
  }

  Console.WriteLine($"Part 1 - Total: {total}");
}

void Part2(string filename)
{
  var lines = File.ReadAllLines(filename);

  ulong total = 0;
  foreach (var line in lines)
  {
    var colonIndex = line.IndexOf(':');
    var testValue = ulong.Parse(line[0..colonIndex]);
    var numbers = line[(colonIndex + 1)..].Split(' ')
      .Where(x => x != "")
      .Select(x => ulong.Parse(x))
      .ToArray();

    var isValid = CanSolveV2(numbers[0], 0, testValue, numbers);
    total += isValid ? testValue : 0;

    Console.WriteLine($"{testValue} - is valid: {isValid}");
  }

  Console.WriteLine($"Part 2 - Total: {total}");
}


bool CanSolve(ulong currentResult, int index, ulong expectedResult, ulong[] numbers)
{
  if (currentResult > expectedResult) return false;
  if (index == numbers.Length - 1) return currentResult == expectedResult;

  if (CanSolve(currentResult + numbers[index + 1], index + 1, expectedResult, numbers))
  {
    return true;
  }

  if (CanSolve(currentResult * numbers[index + 1], index + 1, expectedResult, numbers))
  {
    return true;
  }

  return false;
}

bool CanSolveV2(ulong currentResult, int index, ulong expectedResult, ulong[] numbers)
{
  if (currentResult > expectedResult) return false;
  if (index == numbers.Length - 1) return currentResult == expectedResult;

  if (CanSolveV2(currentResult + numbers[index + 1], index + 1, expectedResult, numbers))
  {
    return true;
  }

  if (CanSolveV2(currentResult * numbers[index + 1], index + 1, expectedResult, numbers))
  {
    return true;
  }

  var concatenatedValue = ulong.Parse($"{currentResult}{numbers[index + 1]}");
  if (CanSolveV2(concatenatedValue, index + 1, expectedResult, numbers))
  {
    return true;
  }

  return false;
}




// Options:
// Find all possible results using one of the following:
// - Recursive backtracking example
// - Use a tree structure
