using System;
using System.Collections.Generic;

namespace TryHards.Suffix
{
  public partial class SuffixArrayLcp<Letter>
     where Letter : IComparable<Letter>
  {
    public readonly ReadOnlyMemory<Letter> Text;

    int[] _suffixArray;
    public ReadOnlyMemory<int> SuffixArray => _suffixArray.AsMemory();

    int[] _lcp;
    public ReadOnlyMemory<int> Lcp => _lcp.AsMemory();

    public IEnumerable<ReadOnlyMemory<Letter>> EnumerateSuffixes()
    {
      foreach (int i in _suffixArray)
        yield return Text[i..];
    }

    public IEnumerable<Range> EnumerateAllUniqueSubstringRanges()
    {
      int n = Text.Length;
      for (int i = 0; i < n; i++)
      {
        var start = _suffixArray[i];
        var lcpValue = (i == 0) ? 0 : _lcp[i - 1];
        var suffixLen = n - start;

        for (int len = lcpValue + 1; len <= suffixLen; len++)
        {
          yield return new Range(start, start + len);
        }
      }
    }

    public IEnumerable<ReadOnlyMemory<Letter>> EnumerateAllUniqueSubstrings()
    {
      foreach (var range in EnumerateAllUniqueSubstringRanges())
        yield return Text[range];
    }

    public int IndexOf(ReadOnlySpan<Letter> substring)
    {
      var text = Text.Span;

      int low = 0;
      int high = _suffixArray.Length - 1;

      while (low <= high)
      {
        var mid = low + (high - low) / 2;
        var suffixStart = _suffixArray[mid];

        var suffixCmp = text[suffixStart..];
        if (suffixCmp.Length > substring.Length)
          suffixCmp = suffixCmp[..substring.Length];

        var cmp = MemoryExtensions.SequenceCompareTo(substring, suffixCmp);
        if (cmp == 0)
          return suffixStart;
        else if (cmp < 0)
          high = mid - 1;
        else
          low = mid + 1;
      }

      return -1;
    }
  }
}