using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TryHards.AhoCorasick;

namespace TryHards.Radix
{
  public class RadixTree<Letter>
  {
    internal bool IsWord;
    internal ReadOnlyMemory<Letter> Prefix;
    internal Dictionary<Letter, RadixTree<Letter>> Edges;

    internal RadixTree<Letter> GetEdge(Letter letter)
    {
      if (Edges != null && Edges.TryGetValue(letter, out var next))
        return next;
      else
        return null;
    }

    internal void InsertEdge(Letter letter, RadixTree<Letter> node)
    {
      Edges ??= new();
      Edges[letter] = node;
    }

    internal RadixTree<Letter> AddEdge(ReadOnlyMemory<Letter> prefix)
    {
      RadixTree<Letter> next = new RadixTree<Letter>
      {
        Prefix = prefix
      };

      InsertEdge(prefix.Span[0], next);
      return next;
    }

    internal IEnumerable<KeyValuePair<Letter, RadixTree<Letter>>> EnumerateEdges()
    {
      if (Edges == null)
        yield break;

      foreach (var kvp in Edges)
        yield return kvp;
    }

    internal int CommonPrefixLength(ReadOnlyMemory<Letter> word1, ReadOnlyMemory<Letter> word2)
    {
      return word1.Span.CommonPrefixLength(word2.Span);
    }

    internal bool StartsWith(ReadOnlyMemory<Letter> full, ReadOnlyMemory<Letter> sub)
    {
      int commonLenth = CommonPrefixLength(full, sub);
      return commonLenth == sub.Length;
    }

    public void Insert(ReadOnlyMemory<Letter> word)
    {
      var current = this;
      int i = 0;
      while (i < word.Length)
      {
        var letter = word.Span[i];

        var child = current.GetEdge(letter);

        if (child == null)
        {
          child = current.AddEdge(word[i..]);
          child.IsWord = true;
          return;
        }

        var childPrefix = child.Prefix;
        int commonLen = CommonPrefixLength(word[i..], childPrefix);
        if (commonLen < childPrefix.Length)
        {
          var newChild = new RadixTree<Letter>
          {
            Prefix = childPrefix[..commonLen]
          };
          child.Prefix = childPrefix[commonLen..];

          newChild.InsertEdge(child.Prefix.Span[0], child);
          current.InsertEdge(letter, newChild);
          child = newChild;
        }

        i += commonLen;
        current = child;
      }

      current.IsWord = true;
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

    public bool Contains(ReadOnlyMemory<Letter> text)
    {
      var current = this;
      int i = 0;
      while (i < text.Length)
      {
        var letter = text.Span[i];
        var child = current.GetEdge(letter);

        if (child == null)
          return false;

        var childPrefix = child.Prefix;
        if (StartsWith(text[i..], childPrefix))
        {
          i += childPrefix.Length;
          current = child;
        }
        else
          return false;
      }

      return current.IsWord;
    }
  }
}