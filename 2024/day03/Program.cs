using System.Text;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Part1("sample.txt");
//Part1("input.txt");

void Part1(string filename)
{
  var raw = File.ReadAllText(filename);

  var sb = new StringBuilder();
  var number = new StringBuilder();
  var numbers = new int[2];
  var total = 0;

  char prev = '\0';
  var functionIsOpen = false;
  foreach (var data in raw)
  {
    if (!IsValidData(data))
    {
      sb.Clear();
      continue;
    }

    sb.Append(data);
    if (data == '(' && sb.ToString() == "mul(" && !functionIsOpen)
    {
      functionIsOpen = true;
      sb.Clear();
      continue;
    }

    if (data == '(' && functionIsOpen)
    {
      functionIsOpen = false;
      sb.Clear();
      number.Clear();
      continue;
    }

    if (IsNumeric(data) && functionIsOpen) 
    {
      number.Append(data);
    }
    if (data == ',' && IsNumeric(prev) && functionIsOpen)
    {
      numbers[0] = int.Parse(number.ToString());
      number.Clear();
    }
    if (data == ')' && IsNumeric(prev) && functionIsOpen)
    {
      numbers[1] = int.Parse(number.ToString());
      Console.WriteLine($"Multiply: {numbers[0]} x {numbers[1]}");
      total += numbers[0] * numbers[1];

      numbers[0] = 0;
      numbers[1] = 0;
      number.Clear();
      sb.Clear();
      functionIsOpen = false;
    }

    prev = data;
  }

  Console.WriteLine($"Part 1 - Total: {total}");
}

bool IsValidData(char data)
{
  return "123456789".Contains(data) || "mul".Contains(data) || "(),".Contains(data);
}

bool IsNumeric(char data) => "123456789".Contains(data);
