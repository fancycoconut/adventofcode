using System.Linq;
using System.Collections.Generic;

namespace Day6;

public class ElfProtocol
{
  private string dataStream;

  public void Load(string dataStream)
  {
    this.dataStream = dataStream;
  }

  public int GetStartOfPacketMarkerPosition()
  {
    return GetMarkerPosition(4);
  }

  public int GetStartOfMessageMarkerPosition()
  {
    return GetMarkerPosition(14);
  }

  public int GetMarkerPosition(int markerSize)
  {
    var index = 0;
    var markerBuffer = new Queue<char>();
    foreach (var character in dataStream.AsSpan())
    {
      index++;
      if (markerBuffer.Count() < markerSize)
      {
        markerBuffer.Enqueue(character);
        continue;
      }

      markerBuffer.Dequeue();
      markerBuffer.Enqueue(character);

      if (HasUniqueItems(markerBuffer) && markerBuffer.Count() == markerSize) return index;
    }

    return index;
  }

  private bool HasUniqueItems(Queue<char> queue)
  {
    var map = new Dictionary<char, int>();
    foreach (var i in queue)
    {
      if (map.ContainsKey(i))
      {
        map[i]++;
        return false;
      }
      else
      {
        map[i] = 0;
      }
    }
    return true;
  }
}
