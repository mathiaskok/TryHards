using System.Collections.Generic;
using System.Linq;
using TryHards.Deque;

namespace TryHards.Tests
{
  [TestFixture]
  public class CircularDequeTests
  {
    [Test]
    public void EmptyWorksAsInteded()
    {
      CircularDeque<int> deq = new();
      Assert.That(deq, Is.Empty);
      Assert.That(deq.Capacity, Is.EqualTo(0));
      Assert.That(deq.TryRemoveEnd(out var _), Is.False);
      Assert.That(deq.TryRemoveFront(out var _), Is.False);
      Assert.That(deq.ToList(), Is.Empty);
    }

    [Test]
    public void AddEnd()
    {
      CircularDeque<int> deq = new(3);
      Assert.That(deq, Is.Empty);
      Assert.That(deq.Capacity, Is.EqualTo(3));

      deq.AddEnd(0);
      deq.AddEnd(1);
      deq.AddEnd(2);

      Assert.That(deq, Has.Count.EqualTo(3));
      Assert.That(deq.Capacity, Is.EqualTo(3));

      List<int> list = deq.ToList();

      Assert.That(list[0], Is.EqualTo(0));
      Assert.That(list[1], Is.EqualTo(1));
      Assert.That(list[2], Is.EqualTo(2));
    }

    [Test]
    public void AddFront()
    {
      CircularDeque<int> deq = new(3);
      Assert.That(deq, Is.Empty);
      Assert.That(deq.Capacity, Is.EqualTo(3));

      deq.AddFront(2);
      deq.AddFront(1);
      deq.AddFront(0);

      Assert.That(deq, Has.Count.EqualTo(3));
      Assert.That(deq.Capacity, Is.EqualTo(3));

      List<int> list = deq.ToList();

      Assert.That(list[0], Is.EqualTo(0));
      Assert.That(list[1], Is.EqualTo(1));
      Assert.That(list[2], Is.EqualTo(2));
    }

    [Test]
    public void CanActAsQueueLeftToRight()
    {
      CircularDeque<int> deq = new(3);

      deq.AddEnd(0);
      deq.AddEnd(1);
      deq.AddEnd(2);

      Assert.That(deq.TryRemoveFront(out var _0), Is.True);
      Assert.That(deq.TryRemoveFront(out var _1), Is.True);
      Assert.That(deq.TryRemoveFront(out var _2), Is.True);

      Assert.That(deq, Is.Empty);
      Assert.That(deq.Capacity, Is.EqualTo(3));

      Assert.That(_0, Is.EqualTo(0));
      Assert.That(_1, Is.EqualTo(1));
      Assert.That(_2, Is.EqualTo(2));
    }

    [Test]
    public void CanActAsQueueRightToLeft()
    {
      CircularDeque<int> deq = new(3);

      deq.AddFront(0);
      deq.AddFront(1);
      deq.AddFront(2);

      Assert.That(deq.TryRemoveEnd(out var _0), Is.True);
      Assert.That(deq.TryRemoveEnd(out var _1), Is.True);
      Assert.That(deq.TryRemoveEnd(out var _2), Is.True);

      Assert.That(deq, Is.Empty);
      Assert.That(deq.Capacity, Is.EqualTo(3));

      Assert.That(_0, Is.EqualTo(0));
      Assert.That(_1, Is.EqualTo(1));
      Assert.That(_2, Is.EqualTo(2));
    }

    [Test]
    public void CanActAsStackLeft()
    {
      CircularDeque<int> deq = new(3);

      deq.AddFront(0);
      deq.AddFront(1);
      deq.AddFront(2);

      Assert.That(deq.TryRemoveFront(out var _2), Is.True);
      Assert.That(deq.TryRemoveFront(out var _1), Is.True);
      Assert.That(deq.TryRemoveFront(out var _0), Is.True);

      Assert.That(deq, Is.Empty);
      Assert.That(deq.Capacity, Is.EqualTo(3));

      Assert.That(_0, Is.EqualTo(0));
      Assert.That(_1, Is.EqualTo(1));
      Assert.That(_2, Is.EqualTo(2));
    }

    [Test]
    public void CanActAsStackRight()
    {
      CircularDeque<int> deq = new(3);

      deq.AddEnd(0);
      deq.AddEnd(1);
      deq.AddEnd(2);

      Assert.That(deq.TryRemoveEnd(out var _2), Is.True);
      Assert.That(deq.TryRemoveEnd(out var _1), Is.True);
      Assert.That(deq.TryRemoveEnd(out var _0), Is.True);

      Assert.That(deq, Is.Empty);
      Assert.That(deq.Capacity, Is.EqualTo(3));

      Assert.That(_0, Is.EqualTo(0));
      Assert.That(_1, Is.EqualTo(1));
      Assert.That(_2, Is.EqualTo(2));
    }

    [Test]
    public void CanGrowCapacityRight()
    {
      CircularDeque<int> deq = new(2);
      Assert.That(deq.Capacity, Is.EqualTo(2));

      deq.AddEnd(0);
      deq.AddEnd(1);
      deq.AddEnd(2);
      deq.AddEnd(3);

      Assert.That(deq, Has.Count.EqualTo(4));
      Assert.That(deq.Capacity, Is.EqualTo(4));

      List<int> list = deq.ToList();
      Assert.That(list[0], Is.EqualTo(0));
      Assert.That(list[1], Is.EqualTo(1));
      Assert.That(list[2], Is.EqualTo(2));
      Assert.That(list[3], Is.EqualTo(3));
    }

    [Test]
    public void CanGrowCapacityLeft()
    {
      CircularDeque<int> deq = new(2);
      Assert.That(deq.Capacity, Is.EqualTo(2));

      deq.AddFront(3);
      deq.AddFront(2);
      deq.AddFront(1);
      deq.AddFront(0);

      Assert.That(deq, Has.Count.EqualTo(4));
      Assert.That(deq.Capacity, Is.EqualTo(4));

      List<int> list = deq.ToList();
      Assert.That(list[0], Is.EqualTo(0));
      Assert.That(list[1], Is.EqualTo(1));
      Assert.That(list[2], Is.EqualTo(2));
      Assert.That(list[3], Is.EqualTo(3));
    }

    [Test]
    public void PartiallyFilledWorks()
    {
      CircularDeque<int> deq = new(10);
      Assert.That(deq.Capacity, Is.EqualTo(10));

      deq.AddEnd(3);
      deq.AddEnd(4);
      deq.AddEnd(5);
      deq.AddFront(2);
      deq.AddFront(1);
      deq.AddFront(0);

      Assert.That(deq, Has.Count.EqualTo(6));
      Assert.That(deq.Capacity, Is.EqualTo(10));

      List<int> list = deq.ToList();
      Assert.That(list[0], Is.EqualTo(0));
      Assert.That(list[1], Is.EqualTo(1));
      Assert.That(list[2], Is.EqualTo(2));
      Assert.That(list[3], Is.EqualTo(3));
      Assert.That(list[4], Is.EqualTo(4));
      Assert.That(list[5], Is.EqualTo(5));
    }

    [Test]
    public void PartiallyFilledWorksQueueLeftToRight()
    {
      CircularDeque<int> deq = new(10);
      Assert.That(deq.Capacity, Is.EqualTo(10));

      deq.AddEnd(3);
      deq.AddEnd(4);
      deq.AddEnd(5);
      deq.AddFront(2);
      deq.AddFront(1);
      deq.AddFront(0);

      Assert.That(deq, Has.Count.EqualTo(6));
      Assert.That(deq.Capacity, Is.EqualTo(10));

      Assert.That(deq.TryRemoveFront(out var _0));
      Assert.That(deq.TryRemoveFront(out var _1));
      Assert.That(deq.TryRemoveFront(out var _2));
      Assert.That(deq.TryRemoveFront(out var _3));
      Assert.That(deq.TryRemoveFront(out var _4));
      Assert.That(deq.TryRemoveFront(out var _5));

      Assert.That(deq, Is.Empty);

      Assert.That(_0, Is.EqualTo(0));
      Assert.That(_1, Is.EqualTo(1));
      Assert.That(_2, Is.EqualTo(2));
      Assert.That(_3, Is.EqualTo(3));
      Assert.That(_4, Is.EqualTo(4));
      Assert.That(_5, Is.EqualTo(5));
    }

    [Test]
    public void PartiallyFilledWorksQueueRightToLeft()
    {
      CircularDeque<int> deq = new(10);
      Assert.That(deq.Capacity, Is.EqualTo(10));

      deq.AddEnd(3);
      deq.AddEnd(4);
      deq.AddEnd(5);
      deq.AddFront(2);
      deq.AddFront(1);
      deq.AddFront(0);

      Assert.That(deq, Has.Count.EqualTo(6));
      Assert.That(deq.Capacity, Is.EqualTo(10));

      Assert.That(deq.TryRemoveEnd(out var _5));
      Assert.That(deq.TryRemoveEnd(out var _4));
      Assert.That(deq.TryRemoveEnd(out var _3));
      Assert.That(deq.TryRemoveEnd(out var _2));
      Assert.That(deq.TryRemoveEnd(out var _1));
      Assert.That(deq.TryRemoveEnd(out var _0));

      Assert.That(deq, Is.Empty);

      Assert.That(_0, Is.EqualTo(0));
      Assert.That(_1, Is.EqualTo(1));
      Assert.That(_2, Is.EqualTo(2));
      Assert.That(_3, Is.EqualTo(3));
      Assert.That(_4, Is.EqualTo(4));
      Assert.That(_5, Is.EqualTo(5));
    }
  }
}