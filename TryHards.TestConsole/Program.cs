using TryHards.AhoCorasick;

namespace TryHards.TestConsole
{
  internal class Program
  {
    static void Main(string[] args)
    {
      Trie<char> trie = new Trie<char>();
      trie.Insert("he".AsMemory());
      trie.Insert("she".AsMemory());
      trie.Insert("his".AsMemory());
      trie.Insert("hers".AsMemory());

      trie.BuildAutomation();

      foreach (var match in trie.FindAllMatches("ushers".AsMemory()))
      {
        System.Console.WriteLine(match.index);
        System.Console.WriteLine(match.pattern.Span.ToString());
        System.Console.WriteLine();
      }
    }
  }
}
