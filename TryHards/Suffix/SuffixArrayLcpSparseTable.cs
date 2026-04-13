using System;

namespace TryHards.Suffix
{
  public partial class SuffixArrayLcpSparseTable<Letter>
    where Letter : IComparable<Letter>, IEquatable<Letter>
  {
    public SuffixArrayLcp<Letter> SuffixArrayLcp;

    int[,] _sparseLcpTable;
    int[] _log;

    public int QuerySparse(int l, int m)
    {
      if (l == m)
        throw new ArgumentException();

      int left = Math.Min(l, m) + 1;
      int right = Math.Max(l, m);

      int j = _log[right - left + 1];
      return Math.Min(
        _sparseLcpTable[j, left],
        _sparseLcpTable[j, right - (1 << j) + 1]);
    }

    public int IndexOf(ReadOnlySpan<Letter> substring)
    {
      var text = SuffixArrayLcp.Text.Span;
      var sa = SuffixArrayLcp.SuffixArray.Span;
      int n = sa.Length;

      if (n == 0 || substring.Length == 0)
        return -1;

      int l = 0;
      int r = n - 1;

      var lcpL = substring.CommonPrefixLength(text[sa[l]..]);
      if (lcpL == substring.Length)
        return sa[l];

      var lcpR = substring.CommonPrefixLength(text[sa[r]..]);
      if (lcpR == substring.Length)
        return sa[r];

      while ((r - l) > 1)
      {
        var m = l + (r - l) / 2;

        if (lcpL >= lcpR)
        {
          var lcpLM = QuerySparse(l, m);

          if (lcpLM > lcpL)
          {
            l = m;
          }
          else if (lcpLM < lcpL)
          {
            r = m;
            lcpR = lcpLM;
          }
          else
          {
            var match = substring[lcpL..].CommonPrefixLength(text[(sa[m] + lcpL)..]);
            var newLcp = lcpL + match;

            if (newLcp == substring.Length)
              return sa[m];

            bool subIsLess =
              (newLcp < text.Length - sa[m]) &&
              substring[newLcp].CompareTo(text[sa[m] + newLcp]) < 0;

            if (subIsLess)
            {
              r = m;
              lcpR = newLcp;
            }
            else
            {
              l = m;
              lcpL = newLcp;
            }
          }
        }
        else
        {
          var lcpMR = QuerySparse(m, r);

          if (lcpMR > lcpR)
          {
            r = m;
          }
          else if (lcpMR < lcpR)
          {
            l = m;
            lcpL = lcpMR;
          }
          else
          {
            var match = substring[lcpR..].CommonPrefixLength(text[(sa[m] + lcpR)..]);
            var newLcp = lcpR + match;

            if (newLcp == substring.Length)
              return sa[m];

            bool subIsLess =
            (newLcp < text.Length - sa[m]) &&
            substring[newLcp].CompareTo(text[sa[m] + newLcp]) < 0;

            if (subIsLess)
            {
              r = m;
              lcpR = newLcp;
            }
            else
            {
              l = m;
              lcpL = newLcp;
            }
          }
        }

        if (text[sa[l]..].StartsWith(substring))
          return sa[l];
        if (text[sa[r]..].StartsWith(substring))
          return sa[r];
      }

      return -1;
    }
  }
}