using System;
using System.Collections.Generic;

namespace TryHards.AhoCorasick
{
  public class Trie<Letter>
  {
    internal Dictionary<Letter, Trie<Letter>> Next;

    internal Trie<Letter> _fail;
    internal Trie<Letter> Fail
    {
      get => _fail;
      set => _fail = value ?? _fail;
    }

    internal bool IsRoot => Fail == null;

    internal List<ReadOnlyMemory<Letter>> Output;

    internal Trie(Trie<Letter> root)
    {
      Fail = root;
    }

    public Trie() { }

    internal void AddNext(Letter letter, Trie<Letter> trie)
    {
      Next ??= new();
      Next.Add(letter, trie);
    }

    internal Trie<Letter> GetNext(Letter letter)
    {
      if (Next != null && Next.TryGetValue(letter, out var next))
        return next;
      else
        return null;
    }

    internal IEnumerable<KeyValuePair<Letter, Trie<Letter>>> GetNextEdges()
    {
      if (Next == null)
        yield break;

      foreach (var kvp in Next)
        yield return kvp;
    }

    internal void AddOutput(ReadOnlyMemory<Letter> pattern)
    {
      Output ??= [];
      Output.Add(pattern);
    }

    internal void AddOutputs(IEnumerable<ReadOnlyMemory<Letter>> patterns)
    {
      Output ??= [];
      Output.AddRange(patterns);
    }

    internal IEnumerable<ReadOnlyMemory<Letter>> GetOutputs()
    {
      if (Output == null)
        yield break;

      foreach (var o in Output)
        yield return o;
    }

    public void Insert(ReadOnlyMemory<Letter> word)
    {
      var current = this;

      foreach (var letter in word.Span)
      {
        var next = current.GetNext(letter);
        if (next == null)
        {
          // This branch could be made faster as we now know that the remaining postfix is missing.
          next = new Trie<Letter>(this);
          current.AddNext(letter, next);
        }

        current = next;
      }

      current.AddOutput(word);
    }

    internal Trie<Letter> FindNearestFailureLink(Letter letter)
    {
      var failLink = Fail;
      Trie<Letter> next;
      while ((next = failLink.GetNext(letter)) == null && !failLink.IsRoot)
        failLink = failLink.Fail;

      return next;
    }

    public void BuildAutomation()
    {
      LinkedList<Trie<Letter>> queue = new LinkedList<Trie<Letter>>();

      foreach (var kvp in GetNextEdges())
        queue.AddLast(kvp.Value);

      while (queue.Count > 0)
      {
        var u = queue.First.Value;
        queue.RemoveFirst();

        foreach (var kvp in u.GetNextEdges())
        {
          var letter = kvp.Key;
          var v = kvp.Value;

          v.Fail = u.FindNearestFailureLink(letter);

          var targetFail = v.Fail;
          if (targetFail != null)
            v.AddOutputs(targetFail.GetOutputs());

          queue.AddLast(v);
        }
      }
    }

    public IEnumerable<(int index, ReadOnlyMemory<Letter> pattern)> FindAllMatches(ReadOnlyMemory<Letter> text)
    {
      var current = this;
      for (int i = 0; i < text.Length; i++)
      {
        var letter = text.Span[i];

        Trie<Letter> next;
        while ((next = current.GetNext(letter)) == null && !current.IsRoot)
          current = current.Fail;

        current = next ?? current;

        foreach(var pattern in current.GetOutputs())
          yield return (i - pattern.Length + 1, pattern);
      }
    }
  }
}