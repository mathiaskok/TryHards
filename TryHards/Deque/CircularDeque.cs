using System;

namespace TryHards.Deque
{
  public partial class CircularDeque<T>
  {
    T[] _buffer;
    int _front;
    int _end;

    public int Capacity => _buffer?.Length ?? 0;
    public int Count { get; private set; }

    public CircularDeque()
    {
      
    }

    public CircularDeque(int initialCapacity)
    {
      if (initialCapacity < 0)
        throw new ArgumentNullException(nameof(initialCapacity));
      else if (initialCapacity > 0)
      {
        _buffer = new T[initialCapacity];
      }
    }

    void Resize()
    {
      if (Capacity == 0)
      {
        _buffer = new T[2];
      }
      else
      {
        T[] newBuffer = new T[_buffer.Length * 2];

        if (_front < _end)
        {
          Array.Copy(_buffer, _front, newBuffer, 0, Count);
        }
        else
        {
          int rightPart = Math.Min(Capacity - _front, Count);
          Array.Copy(_buffer, _front, newBuffer, 0, rightPart);
          Array.Copy(_buffer, 0, newBuffer, rightPart, Count - rightPart);
        }

        _buffer = newBuffer;
        _front = 0;
        _end = Count;
      }
    }

    void ResizeIfNeeded()
    {
      if (Count == Capacity)
        Resize();
    }

    void IncrementFront()
    {
      _front = (_front - 1 + Capacity) % Capacity;
    }

    void DecrementFront()
    {
      _front = (_front + 1 + Capacity) % Capacity;
    }

    void IncrementEnd()
    {
     _end = (_end + 1 + Capacity) % Capacity;
    }
    
    void DecrementEnd()
    {
      _end = (_end - 1 + Capacity) % Capacity;
    }

    public void AddFront(T t)
    {
      ResizeIfNeeded();
      IncrementFront();
      _buffer[_front] = t;
      Count++;
    }

    public bool TryRemoveFront(out T t)
    {
      if (Count > 0)
      {
        t = _buffer[_front];
        _buffer[_front] = default;
        DecrementFront();
        Count--;
        return true;
      }
      else
      {
        t = default;
        return false;
      }
    }

    public void AddEnd(T t)
    {
      ResizeIfNeeded();
      _buffer[_end] = t;
      IncrementEnd();
      Count++;
    }

    public bool TryRemoveEnd(out T t)
    {
      if (Count > 0)
      {
        DecrementEnd();
        t = _buffer[_end];
        _buffer[_end] = default;
        Count--;
        return true;
      }
      else
      {
        t = default;
        return false;
      }
    }
  }
}