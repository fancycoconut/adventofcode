using System.Collections.Generic;

namespace Day9;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var instructions = File.ReadAllLines("input.txt");
        Part1(instructions);
        Part2(instructions);
    }

    public static void Part1(string[] instructions)
    {
        (int x, int y) headCoordinates = (0, 0);
        (int x, int y) tailCoordinates = (0, 0);
        var numberOfVisitedLocations = new HashSet<(int, int)>();        

        numberOfVisitedLocations.Add(tailCoordinates);
        foreach (var instruction in instructions)
        {
            //Console.WriteLine(instruction);

            var instructionPair = instruction.Split(" ");
            var direction = instructionPair[0];
            var amount = int.Parse(instructionPair[1]);

            for (var i = 0; i < amount; i++)
            {
                MoveHead(ref headCoordinates, direction);
                MoveTail(ref tailCoordinates, headCoordinates);
                if (!numberOfVisitedLocations.Contains(tailCoordinates)) numberOfVisitedLocations.Add(tailCoordinates);

                //Console.WriteLine($"Head: ({headCoordinates.x}, {headCoordinates.y})");
                //Console.WriteLine($"Tail: ({tailCoordinates.x}, {tailCoordinates.y})");
            }            
        }

        Console.WriteLine($"Number of visited locations: {numberOfVisitedLocations.Count}");
    }

    public static void Part2(string[] instructions)
    {
        (int x, int y) headCoordinates = (0, 0);
        (int x, int y)[] knots = new (int x, int y)[] {
            (0, 0), (0, 0), (0, 0), (0, 0), (0, 0), (0, 0), (0, 0), (0, 0), (0, 0)
        };
        var numberOfVisitedLocations = new HashSet<(int, int)>();

        numberOfVisitedLocations.Add(knots[^1]);
        foreach (var instruction in instructions)
        {
            var instructionPair = instruction.Split(" ");
            var direction = instructionPair[0];
            var amount = int.Parse(instructionPair[1]);

            for (var i = 0; i < amount; i++)
            {
                MoveHead(ref headCoordinates, direction);
                MoveTail(ref knots[0], headCoordinates);

                for (var j = 1; j < knots.Length; j++)
                {
                    MoveTail(ref knots[j], knots[j - 1]);
                }
                if (!numberOfVisitedLocations.Contains(knots[^1])) numberOfVisitedLocations.Add(knots[^1]);
            }            
        }

        Console.WriteLine($"Number of visited locations: {numberOfVisitedLocations.Count}");
    }

    public static void MoveHead(ref (int x, int y) coordinates, string direction)
    {
        switch (direction)
        {
            case "U":
                coordinates.y++;
                break;
            case "D":
                coordinates.y--;
                break;
            case "L":
                coordinates.x--;
                break;
            case "R":
                coordinates.x++;
                break;
        }
    }

    public static void MoveTail(ref (int x, int y) tailCoordinates, (int x, int y) targetCoordinates)
    {
        var deltaX = targetCoordinates.x - tailCoordinates.x;
        var deltaY = targetCoordinates.y - tailCoordinates.y;

        // If touching
        if (Math.Abs(deltaX) <= 1 && Math.Abs(deltaY) <= 1)
        {
            return;
        }

        tailCoordinates.x += Math.Sign(deltaX);
        tailCoordinates.y += Math.Sign(deltaY);
    }
}
// using System.ComponentModel;
// using System.Runtime.CompilerServices;

// public class Program
// {
//     public static void Main(string[] args)
//     {
//         var lines = File.ReadAllLines("input.txt");
//         var i = Day9.ProcessPart2(lines);
//         Console.WriteLine(i);
//     }
// }

// public static class Day9
// {
//     public static int ProcessPart2(string[] input)
//     {
//         var visited = new HashSet<(int, int)>()
//         {
//              // Visit first position
//              (0, 0)
//         };

//         // Create head + knots
//         (int X, int Y) headPos = (0, 0);
//         (int X, int Y)[] knots = {
//             (0, 0),
//             (0, 0),
//             (0, 0),
//             (0, 0),
//             (0, 0),
//             (0, 0),
//             (0, 0),
//             (0, 0),
//             (0, 0),
//         };

//         for (var i = 0; i < input.Length; i++)
//         {
//             // Decode input
//             var instruction = input[i];
//             var dir = instruction[0];
//             var dis = int.Parse(instruction[2..]);

//             // For each step
//             for (var j = 0; j < dis; j++)
//             {
//                 // Move head
//                 headPos.MoveHead(dir);
//                 // First knot follows head
//                 knots[0].Follow(headPos);
//                 // Other knots follow prev knot
//                 for (int k = 1; k < knots.Length; k++)
//                 {
//                     knots[k].Follow(knots[k - 1]);
//                 }
//                 // Visit tail pos
//                 visited.TryAdd(knots[^1]);
//             }
//         }
//         return visited.Count;
//     }

// }