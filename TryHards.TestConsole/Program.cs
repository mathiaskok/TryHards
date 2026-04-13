using TryHards.Radix;
using TryHards.Suffix;

namespace TryHards.TestConsole
{
  internal class Program
  {
    static void Main(string[] args)
    {
      const string text = "banana";
      SuffixArrayLcp<char> bananaStuff = new SuffixArrayLcp<char>(text.AsMemory());
      var a = bananaStuff
        .EnumerateSuffixes()
        .Select(t => t.ToString())
        .ToList();

      var b = bananaStuff.EnumerateAllUniqueSubstrings().Select(s => s.ToString()).ToList();

      
      var c_1 = bananaStuff.IndexOf("nan");
      var d_1 = bananaStuff.IndexOf("banana");
      var e_1 = bananaStuff.IndexOf("ba");
      var f_1 = bananaStuff.IndexOf("na");
      var g_1 = bananaStuff.IndexOf("ana");

      var b2 = new SuffixArrayLcpSparseTable<char>(bananaStuff);
      var c_2 = b2.IndexOf("nan");
      var d_2 = b2.IndexOf("banana");
      var e_2 = b2.IndexOf("ba");
      var f_2 = b2.IndexOf("na");
      var g_2 = b2.IndexOf("ana");
    }
  }
}
