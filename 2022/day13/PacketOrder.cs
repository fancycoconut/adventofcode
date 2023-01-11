using System.Collections.Generic;

namespace day13;

public class PacketOrder
{
    private bool[] expected = { true, true, false, true, false, true, false, false };

    public void PrintSumOfIndicies(string input)
    {
        var lines = File.ReadAllLines(input);
        var signalResults = Process(lines).ToList();

        var i = 0;
        var sumOfIndicies = 0;
        foreach (var result in signalResults)
        {
            i++;
            sumOfIndicies += result ? i : 0;
        }
        
        Console.WriteLine($"Sum of Indicies: {sumOfIndicies}");
    }

    private IEnumerable<bool> Process(string[] lines)
    {
        var index = 0;
        for (var i = 0; i < lines.Length; i += 3)
        {
            var left = BuildList(lines[i]);
            var right = BuildList(lines[i + 1]);

            var correctOrder = InRightOrder(left, right);
            Console.WriteLine($"{index}: {correctOrder == expected[index]}");
            yield return correctOrder;
            index++;
        }
    }

    private List<object> BuildList(string line)
    {
        var i = 0;
        List<object> final = null!;
        var stack = new Stack<List<object>>();

        while (i < line.Length)
        {
            var character = line[i];
            i++;

            if (character == ',') continue;
            if (character == ']')
            {
                final = stack.Pop();
                continue;
            }
            if (character == '[')
            {
                var newList = new List<object>();
                if (stack.Count > 0)
                {
                    stack.Peek().Add(newList);
                }
                
                stack.Push(newList);
                continue;
            }

            stack.Peek().Add(character);
        }

        return final;
    }

    private bool InRightOrder(List<object> left, List<object> right)
    {
        for (var i = 0; i < left.Count; i++)
        {
            // There's still items on the right but items are ordered on the left
            //if (i >= right.Count) return true;

            var a = TryGetValue(left, i);
            var b = TryGetValue(right, i);
            var leftIsValue = a is char;
            var rightIsValue = b is char;

            if (leftIsValue && !rightIsValue)
            {
                var listB = (List<object>)b;
                var rightOrder = InRightOrder(new List<object> { (char)a }, listB);
                if (!rightOrder) return false;
            }

            if (!leftIsValue && rightIsValue)
            {
                var listA = (List<object>)a;
                var rightOrder = InRightOrder(listA, new List<object> { (char)b });
                if (!rightOrder) return false;
            }

            if (!leftIsValue && !rightIsValue)
            {
                if (i == left.Count - 1 && left.Count < right.Count) return true;
                if (i > right.Count) return false;

                var listA = (List<object>)a;
                var listB = (List<object>)b;

                var rightOrder = InRightOrder(listA, listB);
                if (!rightOrder) return false;
;           }

            if (leftIsValue && rightIsValue)
            {
                if (i >= right.Count && (char)a < (char)right[^0]) return true;

                if ((char)a > (char)b) return false;
            }
        }

        if (left.Count == right.Count) return true;

        return left.Count < right.Count;
    }

    private object TryGetValue(List<object> list, int index)
    {
        try
        {
            return list[index];
        }
        catch
        {
            return (char)0;
        }
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
