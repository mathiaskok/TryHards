using TryHards.AhoCorasick;

namespace TryHards.TestConsole
{
  internal class Program
  {
    static void Main(string[] args)
    {
      Trie<char> trie = new Trie<char>();
      trie.Insert("pin".AsMemory());
      trie.Insert("apple".AsMemory());
      trie.Insert("applepie".AsMemory());

      trie.BuildAutomation();

      foreach (var match in trie.FindAllMatches("pineapplepie".AsMemory()))
      {
        System.Console.WriteLine(match.index);
        System.Console.WriteLine(match.pattern.Span.ToString());
        System.Console.WriteLine();
      }
    }
  }
}
