// // See https://aka.ms/new-console-template for more information
// using System.Text.Json;
// using System.Xml.Xsl;
//
// Console.WriteLine("Hello, World!");
//
// var text = File.ReadAllText("sample.txt");
//
// var data = text
//     .Split("\n\n")
//     .Select(line => line.Split("\n"))
//     .Select(pair => (left: JsonDocument.Parse(pair[0]).RootElement, right: JsonDocument.Parse(pair[1]).RootElement))
//     .ToList();
//
// var index = 0;
// var sumOfIndices = 0;
// foreach (var pair in data)
// {
//     index++;
//     var correctOrder = InRightOrder(pair.left, pair.right);
//     Console.WriteLine($"{index}: {correctOrder}");
//     
//     if (correctOrder != false) sumOfIndices += index;
// }
//
// Console.WriteLine($"Part 1: {sumOfIndices}");
//
// text += "\n[[2]]\n[[6]]";
// var data2 = text.Replace("\n\n", "\n")
//     .Split("\n")
//     .Select(line => JsonDocument.Parse(line).RootElement)
//     .ToList();
//
// data2.Sort((a, b) => InRightOrder(a, b) switch
// {
//     true => -1,
//     false => 1,
//     _ => 0
// });
//
// //data2.ForEach(x => Console.WriteLine(x.ToString()));
//
// var data3 = data2.Select(x => x.ToString())
//     .ToList();
//
// var index1 = data3.IndexOf("[[2]]") + 1;
// var index2 = data3.IndexOf("[[6]]") + 1;
//
// Console.WriteLine($"Part 2: {index1}, {index2}, {index1 * index2}");
//
// bool? InRightOrder(JsonElement left, JsonElement right)
// {
//     switch (left.ValueKind)
//     {
//         case JsonValueKind.Array:
//             return right.ValueKind == JsonValueKind.Array
//                 ? CheckArrays(left.EnumerateArray(), right.EnumerateArray())
//                 : CheckArrays(left.EnumerateArray(), new[] { right });
//         case JsonValueKind.Number:
//             return right.ValueKind == JsonValueKind.Array
//                 ? CheckArrays(new[] { left }, right.EnumerateArray())
//                 : left.GetInt32() < right.GetInt32()
//                     ? true
//                     : left.GetInt32() > right.GetInt32()
//                         ? false
//                         : null;
//     }
//
//     return true;
// }
//
// bool? CheckArrays(IEnumerable<JsonElement> left, IEnumerable<JsonElement> right)
// {
//     var leftList = left.ToList();
//     var rightList = right.ToList();
//     
//     for (var i = 0; i < leftList.Count && i < rightList.Count; i++)
//     {
//         var leftItem = leftList[i];
//         var rightItem = rightList[i];
//         var rightOrder = InRightOrder(leftItem, rightItem);
//         if (rightOrder.HasValue) return rightOrder.Value;
//     }
//
//     return leftList.Count == rightList.Count
//         ? null
//         : leftList.Count < rightList.Count;
// }