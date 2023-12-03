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
    var lines = File.ReadAllLines("sample.txt");

    var sum = 0;
    var sb = new StringBuilder();
    foreach (var line in lines)
    {
        int first = 0, last = 0;
        foreach (var c in line.AsSpan())
        {
            sb.Append(c);
            //Console.WriteLine($"Test: {sb.ToString()}");
            var digit = GetDigit(sb.ToString());
            if (digit == 0) continue;
            sb.Clear();

            if (first == 0) first = digit;
            last = digit;
        }

        var number = first.ToString() + last.ToString();
        Console.WriteLine(number);

        sum += Convert.ToInt32($"{first}{last}");
    }

    Console.WriteLine($"The answer is: {sum}");
}

static int GetDigit(string numberAsText)
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