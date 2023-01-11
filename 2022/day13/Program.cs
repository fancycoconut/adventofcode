using day13;

namespace Day13;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");

        var packetOrder = new PacketOrder();
        packetOrder.PrintSumOfIndicies("sample.txt");
    }
}


// using System.Text.Json.Nodes;
//
// var input = await File.ReadAllTextAsync("input.txt");
//
// var pairs = input.Split("\r\n\r\n");
// var pairIndex = 0;
// var correctPairs = 0;
//
// foreach (var pair in pairs)
// {
//     pairIndex++;
//
//     var splitPair = pair.Split("\r\n");
// var left = splitPair[0];
// var right = splitPair[1];
// var jsonLeft = JsonNode.Parse(left);
// var jsonRight = JsonNode.Parse(right);
// var isCorrect = Compare(jsonLeft, jsonRight);
//     if (isCorrect == true) correctPairs += pairIndex;
// }
//
// Console.WriteLine($"Part 1: {correctPairs}");
//
// var allPackets = input.Split("\r\n").Where(l => !string.IsNullOrEmpty(l)).Select(l => JsonNode.Parse(l)).ToList();
// var x = JsonNode.Parse("[[2]]");
// var y = JsonNode.Parse("[[6]]");
//
// allPackets.Add(x);
// allPackets.Add(y);
//
// allPackets.Sort((left, right) => Compare(left, right) == true ? -1 : 1);
//
// Console.WriteLine($"Part 2: {(allPackets.IndexOf(x) + 1) * (allPackets.IndexOf(y) + 1)}");
//
// bool? Compare(JsonNode left, JsonNode right)
// {
//     if (left is JsonValue leftVal && right is JsonValue rightVal)
//     {
//         var leftInt = leftVal.GetValue<int>();
//         var rightInt = rightVal.GetValue<int>();
//         return leftInt == rightInt ? null : leftInt < rightInt;
//     }
//
//     if (left is not JsonArray leftArray) leftArray = new JsonArray(left.GetValue<int>());
//     if (right is not JsonArray rightArray) rightArray = new JsonArray(right.GetValue<int>());
//
//     for (var i = 0; i < Math.Min(leftArray.Count, rightArray.Count); i++)
//     {
//         var res = Compare(leftArray[i], rightArray[i]);
//         if (res.HasValue) { return res.Value; }
//     }
//
//     if (leftArray.Count < rightArray.Count) { return true; }
//     if (leftArray.Count > rightArray.Count) { return false; }
//
//     return null;
//}