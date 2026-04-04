using System;
using System.Collections.Generic;

namespace TryHards.Radix
{
  public class RadixTree<Letter>
  {
    internal RadixTreeNode<Letter> Root;
    internal IEqualityComparer<Letter> Comparer;

    public RadixTree(
      IEnumerable<ReadOnlyMemory<Letter>> words,
      bool compact,
      IEqualityComparer<Letter> comparer)
    {
      Comparer = comparer ?? EqualityComparer<Letter>.Default;
      Root = new();

      if (words != null)
      {
        foreach (var word in words)
          Insert(word);
      }

      if (compact)
        Compact();
    }

    public RadixTree(
      IEnumerable<ReadOnlyMemory<Letter>> words,
      bool compact)
      : this(words, compact, null) { }

    public RadixTree(IEqualityComparer<Letter> comparer)
      : this(null, false, comparer) { }

    public RadixTree()
      : this(null) { }

    internal int CommonPrefixLength(ReadOnlyMemory<Letter> word1, ReadOnlyMemory<Letter> word2)
    {
      return word1.Span.CommonPrefixLength(word2.Span, Comparer);
    }

    internal bool StartsWith(ReadOnlyMemory<Letter> full, ReadOnlyMemory<Letter> sub)
    {
      int commonLenth = CommonPrefixLength(full, sub);
      return commonLenth == sub.Length;
    }

    public void Insert(ReadOnlyMemory<Letter> word)
    {
      var current = Root;
      int i = 0;
      while (i < word.Length)
      {
        var letter = word.Span[i];

        var child = current.GetEdge(letter);

        if (child == null)
        {
          child = current.AddEdge(word[i..], Comparer);
          child.IsWord = true;
          return;
        }

        var childPrefix = child.Prefix;
        int commonLen = CommonPrefixLength(word[i..], childPrefix);
        if (commonLen < childPrefix.Length)
        {
          var newChild = new RadixTreeNode<Letter>
          {
            Prefix = childPrefix[..commonLen]
          };
          child.Prefix = childPrefix[commonLen..];

          newChild.InsertEdge(child.Prefix.Span[0], child, Comparer);
          current.InsertEdge(letter, newChild, Comparer);
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
    public void Compact() =>
      Root.Compact();

    public bool ContainsKey(ReadOnlyMemory<Letter> text)
    {
      var current = Root;
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