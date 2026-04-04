using System;
using System.Collections.Generic;

namespace TryHards.Radix
{
  public class RadixTreeNode<Letter>
  {
    public bool IsWord;
    public ReadOnlyMemory<Letter> Prefix;
    public Dictionary<Letter, RadixTreeNode<Letter>> Edges;

    public RadixTreeNode<Letter> GetEdge(Letter letter)
    {
      if (Edges != null && Edges.TryGetValue(letter, out var next))
        return next;
      else
        return null;
    }

    public void InsertEdge(Letter letter, RadixTreeNode<Letter> node, IEqualityComparer<Letter> comparer)
    {
      Edges ??= new(comparer);
      Edges[letter] = node;
    }

    public RadixTreeNode<Letter> AddEdge(ReadOnlyMemory<Letter> prefix, IEqualityComparer<Letter> comparer)
    {
      RadixTreeNode<Letter> next = new RadixTreeNode<Letter>
      {
        Prefix = prefix
      };

      InsertEdge(prefix.Span[0], next, comparer);
      return next;
    }

    public IEnumerable<KeyValuePair<Letter, RadixTreeNode<Letter>>> EnumerateEdges()
    {
      if (Edges == null)
        yield break;

      foreach (var kvp in Edges)
        yield return kvp;
    }

    /// <summary>
    /// This is not guaranteed to reduce memory usage.
    /// Mostly relevant if multiple dynamic strings have been added.
    /// If static string literals are dominant, then this will likely increase memory usage.
    /// Also only relevant is tree is expected to be long lived.
    /// </summary>
    public void Compact()
    {
      if (Prefix.Length > 0)
      {
        var prefix = new Letter[Prefix.Length];
        Prefix.Span.CopyTo(prefix.AsSpan());
        Prefix = prefix;
      }

      foreach (var edge in EnumerateEdges())
        edge.Value.Compact();
    }
  }
}