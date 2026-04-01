using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace TryHards.Dictionary
{
  public class Trie<Letter>
  {
    bool IsWord;

    Dictionary<Letter, Trie<Letter>> Edges;

    public Trie()
    {

    }

    public void Insert(ReadOnlySpan<Letter> word)
    {
      var current = this;

      foreach (var letter in word)
      {
        if (current.Edges == null)
          current.Edges = new Dictionary<Letter, Trie<Letter>>();

        if (!current.Edges.TryGetValue(letter, out var next))
        {
          // This branch could be made faster as we now know that the remaining postfix is missing.
          next = new Trie<Letter>();
          current.Edges.Add(letter, next);
        }

        current = next;
      }

      current.IsWord = true;
    }

    Trie<Letter> FindWordLeafNode(ReadOnlySpan<Letter> word)
    {
      var current = this;
      foreach (var letter in word)
      {
        var edges = current.Edges;
        if (edges == null || !edges.TryGetValue(letter, out current))
          return null;
      }
      return current;
    }

    public bool Contains(ReadOnlySpan<Letter> word)
    {
      var node = FindWordLeafNode(word);
      return node != null && node.IsWord;
    }

    public (int index, Trie<Letter> node) FindLongestPrefix(ReadOnlySpan<Letter> word)
    {
      var current = this;
      int i = 0;
      for (; i < word.Length; i++)
      {
        var edges = current.Edges;
        if (edges == null || !edges.TryGetValue(word[i], out var next))
          break;
        else
          current = next;
      }

      return (i - 1, current);
    }
  }
}
