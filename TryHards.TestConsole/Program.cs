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
    }
  }
}
