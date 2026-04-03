using System;
using System.Collections.Generic;

namespace TryHards.AhoCorasick
{
  public class Trie<Letter>
  {
    internal Dictionary<Letter, Trie<Letter>> Next;
    internal Trie<Letter> Fail;
    internal List<ReadOnlyMemory<Letter>> Output;

    internal Trie(Trie<Letter> root)
    {
      Fail = root;
    }

    public Trie() { }

    public void Insert(ReadOnlyMemory<Letter> word)
    {
      var current = this;

      foreach (var letter in word.Span)
      {
        if (current.Next == null)
          current.Next = new();

        if (!current.Next.TryGetValue(letter, out var next))
        {
          // This branch could be made faster as we now know that the remaining postfix is missing.
          next = new Trie<Letter>(this);
          if (current.Next == null)
            current.Next = new();

          current.Next.Add(letter, next);
        }

        current = next;
      }

      if (current.Output == null)
        current.Output = new();

      current.Output.Add(word);
    }

    public void BuildAutomation()
    {
      LinkedList<Trie<Letter>> queue = new LinkedList<Trie<Letter>>();

      if (Next != null)
      {
        foreach (var next in Next.Values)
          queue.AddLast(next);
      }

      while (queue.Count > 0)
      {
        var u = queue.First.Value;
        queue.RemoveFirst();

        if (u.Next != null)
        {
          foreach (var kvp in u.Next)
          {
            var letter = kvp.Key;
            var v = kvp.Value;

            var failLink = u.Fail;
            while (failLink?.Next != null && !failLink.Next.ContainsKey(letter))
              failLink = failLink.Fail;

            v.Fail = failLink?.Next[letter];

            var targetFail = v.Fail;
            if (targetFail != null)
            {
              var o = targetFail.Output;
              if (o != null)
              {
                if (v.Output == null)
                  v.Output = new();

                v.Output.AddRange(targetFail.Output);
              }
            }

            queue.AddLast(v);
          }
        }
      }
    }

    public IEnumerable<(int index, ReadOnlyMemory<Letter> pattern)> FindAllMatches(ReadOnlyMemory<Letter> text)
    {
      var current = this;
      for (int i = 0; i < text.Length; i++)
      {
        var letter = text.Span[i];

        Trie<Letter> next = null;
        while (current.Next == null || !current.Next.TryGetValue(letter, out next))
        {
          if (current.Fail == null)
            break;
          else
            current = current.Fail;
        }

        current = next ?? current;

        if (current.Output != null)
        {
          foreach (var pattern in current.Output)
            yield return (i - pattern.Length + 1, pattern);
        }
      }
    }
  }
}