namespace Day4;

public class ElfAssignmentManager
{
  private string[] assignments;

  public void Load(string input)
  {
    assignments = File.ReadAllLines(input);
  }

  public int CalculateFullyCoveredAssignments()
  {
    var count = 0;
    foreach (var assignment in assignments)
    {
      var assignmentPair = assignment.Split(",");
      var pair1 = (new Assignment(assignmentPair[0]))
        .ToArray();
      var pair2 = (new Assignment(assignmentPair[1]))
        .ToArray();
      //Console.WriteLine(string.Join(", ", pair1));
      //Console.WriteLine(string.Join(", ", pair2));

      if (!pair1.Except(pair2).Any() || !pair2.Except(pair1).Any()) {
        count++;
      }
    }

    return count;
  }
}