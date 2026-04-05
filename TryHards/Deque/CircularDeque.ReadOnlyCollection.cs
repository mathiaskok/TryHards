using System.Collections;
using System.Collections.Generic;

namespace TryHards.Deque
{
  public partial class CircularDeque<T> : IReadOnlyCollection<T>
  {
    public IEnumerator<T> GetEnumerator()
    {
      int idx = _front;
      for (int i = 0; i < Count; i++)
      {
        yield return _buffer[idx];
        idx = (idx + 1 + Capacity) % Capacity;
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}