using System;
using System.Collections.Generic;
using System.Linq;

namespace TryHards.Suffix
{
  public partial class SuffixArrayLcp<Letter>
  {
    class RankOrder : IComparer<int>
    {
      public int[] ranks;
      public int k;
      public int n;

      public (int, int) CreatePair(int x)
      {
        return (
          ranks[x],
          x + k < n ? ranks[x + k] : -1);
      }

      public int Compare(int x, int y)
      {
        var pairX = CreatePair(x);
        var pairY = CreatePair(y);
        return pairX.CompareTo(pairY);
      }
    }

    public SuffixArrayLcp(ReadOnlyMemory<Letter> text)
    {
      Text = text;
      _lcp = new int[text.Length - 1];

      _suffixArray = new int[text.Length];
      for (int i = 0; i < text.Length; i++)
        _suffixArray[i] = i;

      PopulateLcpKasai(SortSuffixArray());
    }

    int[] CreateInitialRanks()
    {
      var text = Text.Span;
      int[] ranks = new int[Text.Length];

      SortedSet<Letter> alphabet = [.. text];
      Dictionary<Letter, int> rankedAlphabet = new(alphabet.Count);
      foreach (var (l, i) in alphabet.Select((l, i) => (l, i)))
        rankedAlphabet.Add(l, i);

      for (int i = 0; i < text.Length; i++)
        ranks[i] = rankedAlphabet[text[i]];

      return ranks;
    }

    int[] SortSuffixArray()
    {
      int n = Text.Length;
      int[] ranks = CreateInitialRanks();
      int[] nextRanks = new int[ranks.Length];
      int k = 1;

      RankOrder cmp = new()
      {
        n = n
      };

      while (k < n)
      {
        cmp.k = k;
        cmp.ranks = ranks;
        Array.Sort(_suffixArray, cmp);

        nextRanks[0] = 0;
        int currentRankValue = 0;
        for (int i = 1; i < n; i++)
        {
          var currentIdx = _suffixArray[i];
          var prevIdx = _suffixArray[i - 1];

          var currentPair = cmp.CreatePair(currentIdx);
          var prevPair = cmp.CreatePair(prevIdx);

          if (currentPair.CompareTo(prevPair) > 0)
            currentRankValue += 1;

          nextRanks[currentIdx] = currentRankValue;
        }
        (nextRanks, ranks) = (ranks, nextRanks);

        // Optimization: If all ranks are unique, we are done
        if (ranks[_suffixArray[n - 1]] == n - 1)
          break;

        k *= 2;
      }

      return ranks;
    }

    void PopulateLcpKasai(int[] ranks)
    {
      var letterCmp = EqualityComparer<Letter>.Default;
      var n = Text.Length;
      var text = Text.Span;

      for (int i = 0; i < n; i++)
        ranks[_suffixArray[i]] = i;

      int h = 0;
      for (int i = 0; i < n; i++)
      {
        if (ranks[i] > 0)
        {
          int j = _suffixArray[ranks[i] - 1];
          while ((i + h) < n && (j + h) < n && letterCmp.Equals(text[i + h], text[j + h]))
            h++;

          _lcp[ranks[i] - 1] = h;

          if (h > 0)
            h--;
        }
      }
    }
  }
}