using System;

namespace TryHards.Suffix
{
  public partial class SuffixArrayLcpSparseTable<Letter>
  {  
    public SuffixArrayLcpSparseTable(SuffixArrayLcp<Letter> sa)
    {
      SuffixArrayLcp = sa;
      BuildLog();
      BuildSparseLcpTable();
    }

    void BuildLog()
    {
      var n = SuffixArrayLcp.Lcp.Length;
      _log = new int[n + 1];

      _log[1] = 0;
      for (int i = 2; i <= n; i++)
        _log[i] = _log[i / 2] + 1;
    }

    void BuildSparseLcpTable()
    {
      var lcp = SuffixArrayLcp.Lcp.Span;
      var n = lcp.Length;

      int maxLog = _log[n] + 1;
      _sparseLcpTable = new int[maxLog, n];

      for (int i = 0; i < n; i++)
        _sparseLcpTable[0, i] = lcp[i];

      for (int j = 1; j < maxLog; j++)
      {
        for (int i = 0; i + (1 << j) <= n; i++)
        {
          _sparseLcpTable[j, i] = Math.Min(
            _sparseLcpTable[j - 1, i],
            _sparseLcpTable[j - 1, i + (1 << (j - 1))]);
        }
      }
    }
  }
}