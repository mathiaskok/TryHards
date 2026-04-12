using System;
using System.Collections.Generic;

namespace TryHards.Suffix
{
  public partial class SuffixArrayLcp<Letter>
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
  }
}