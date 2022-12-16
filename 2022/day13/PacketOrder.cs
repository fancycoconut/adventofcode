using System.Collections;

namespace day13;

public class PacketOrder
{
    public List<bool> SignalPairsInOrder = new();

    public void Process(string input)
    {
        var lines = File.ReadAllLines(input);
        SignalPairsInOrder = Process(lines).ToList();
    }

    private IEnumerable<bool> Process(string[] lines)
    {
        var index = 0;
        for (var i = 0; i < lines.Length; i += 3)
        {
            var left = GetList(lines[i]);
            var right = GetList(lines[i + 1]);

            //var correctOrder = InRightOrder(left, right);
            yield return true;
            index++;
        }
    }

    private List<object> GetList(string line)
    {
        var i = 1;
        var list = new List<object>();
        var indexSum = 0;
        while (i < line.Length - 1)
        {
            var character = line[i];
            i++;

            if (character == ',' || character == ']') continue;
            if (character == '[')
            {
                var prev = i - 1;
                var index = line[prev..].IndexOf(']') + 1;
                var sublist = line[prev..][..index];
                i = index + indexSum + 1;
                indexSum += index;
                list.Add(GetList(sublist));
                continue;
            }

            list.Add(character);
        }

        return list;
    }

    private bool InRightOrder(string left, string right)
    {
        var max = new[] { left.Length, right.Length }.Max();
        for (var i = 0; i < max; i++)
        {
            var a = i < left.Length ? left[i] : int.MaxValue;
            var b = i < right.Length ? right[i] : int.MaxValue;
            if (a == int.MaxValue || b == int.MaxValue) break;

            if ((a == '[' && b == '[') || (a == ']' && b == ']')) continue;
            if ((a == ',' && b == ',') || (a == ',' && b == ',')) continue;
            if (a == '[' && b != '[')
            {
                var closingBracketIndexForOtherList = right[i..^0].IndexOf(']');
                var newRight = $"{right[0..i]}[{right[i..^closingBracketIndexForOtherList]}]{right[i..^0][closingBracketIndexForOtherList..]}";
                return InRightOrder(left, newRight);
            }
            if (a != '[' && b == '[')
            {
                var closingBracketIndexForOtherList = left[i..^0].IndexOf(']');
                var newLeft = $"{left[0..i]}[{left[i..^closingBracketIndexForOtherList]}]{left[i..^0][closingBracketIndexForOtherList..]}";
                return InRightOrder(newLeft, right);
            }
            if (a == ']' && i == left.Length - 1) return true;
            if (b == ']' && a >= 48 && a <= 57) return false;

            if (a > b) return false;
        }

        return true;
    }
}
