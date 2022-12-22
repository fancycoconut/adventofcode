using System.Collections.Generic;

namespace day13;

public class PacketOrder
{
    public List<bool> SignalPairsInOrder = new();

    public void Process(string input)
    {
        var lines = File.ReadAllLines(input);
        SignalPairsInOrder = Process(lines).ToList();
        //true, true, false, true, false, true, false, false
    }

    private IEnumerable<bool> Process(string[] lines)
    {
        var index = 0;
        for (var i = 0; i < lines.Length; i += 3)
        {
            var left = BuildList(lines[i]);
            var right = BuildList(lines[i + 1]);

            var correctOrder = InRightOrder(left, right);
            Console.WriteLine($"{index}: {correctOrder}");
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
            //if (i == left.Count - 1 && right.Count > left.Count) return true;
            //if (i >= right.Count) return true;
            
            var a = TryGetValue(left, i);
            var b = TryGetValue(right, i);
            var leftIsValue = a is char;
            var rightIsValue = b is char;

            // if (i >= right.Count)
            // {
            //     if (i == right.Count - 1)
            //     {
            //         
            //     }
            //
            //     break;
            // }

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
                var listA = (List<object>)a;
                var listB = (List<object>)b;
                if (!listB.Any()) return false;
                
                var rightOrder = InRightOrder(listA, listB);
                if (!rightOrder) return false;
;           }

            if (leftIsValue && rightIsValue)
            {
                //if (i >= right.Count - 1 && (char)b == char.MaxValue) return false;
                if ((char)a > (char)b) return false;
            }
        }

        return true;
    }

    private object TryGetValue(List<object> list, int index)
    {
        try
        {
            return list[index];
        }
        catch
        {
            return char.MaxValue;
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
