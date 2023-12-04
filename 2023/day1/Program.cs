// See https://aka.ms/new-console-template for more information
using System.Text;

Console.WriteLine("Hello, World!");

//PartOne();
PartTwo();

static void PartOne()
{
    var lines = File.ReadAllLines("sample.txt");

    var sum = 0;
    foreach (var line in lines)
    {
        char first = ' ';
        char last = ' ';
        foreach (var c in line.AsSpan())
        {
            if (int.TryParse(c.ToString(), out var val) == false) continue;

            if (first == ' ') first = c;

            last = c;
        }

        var number = first.ToString() + last.ToString();
        //Console.WriteLine(number);

        sum += Convert.ToInt32(number);
    }

    Console.WriteLine($"The answer is: {sum}");
}

static void PartTwo()
{
    var lines = File.ReadAllLines("input.txt");

    var sum = 0;
    foreach (var line in lines)
    {
        var digits = GetDigits(line).ToArray();
        Console.WriteLine($"{digits[0]}{digits[digits.Length - 1]}");

        sum += Convert.ToInt32($"{digits[0]}{digits[digits.Length - 1]}");
    }

    Console.WriteLine($"The answer is: {sum}");
}

static IEnumerable<int> GetDigits(string line)
{
    var sb = new StringBuilder();
    foreach (var c in line)
    {
        if (int.TryParse(c.ToString(), out var val)) {
            yield return val;
            continue;
        }

        sb.Append(c);
        var digit = GetDigit(sb.ToString());

        if (digit == 0) continue;
        yield return digit;

        sb.Clear();
        sb.Append(c);
    }
}

static int GetDigit(string numberAsText)
{
    var numbers = new string[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
    foreach (var number in numbers) {
        if (numberAsText.Contains(number)) return GetNumber(number);
    }

    return 0;
}

static int GetNumber(string numberAsText)
{
    return numberAsText switch
    {
        "one" => 1,
        "two" => 2,
        "three" => 3,
        "four" => 4,
        "five" => 5,
        "six" => 6,
        "seven" => 7,
        "eight" => 8,
        "nine" => 9,
        _ => 0
    };
}