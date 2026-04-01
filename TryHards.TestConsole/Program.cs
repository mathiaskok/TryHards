using TryHards.Dictionary;

namespace TryHards.TestConsole
{
  internal class Program
  {
    static void Main(string[] args)
    {
      Trie<char> trie = new Trie<char>();
      trie.Insert("asdf");
      trie.Insert("hjk");
      trie.Insert("1234");
      trie.Insert("as");
      trie.Insert("fuck");

      Console.WriteLine(trie.Contains("asdf"));
      Console.WriteLine(trie.Contains("hjk"));
      Console.WriteLine(trie.Contains("1234"));
      Console.WriteLine(trie.Contains("as"));
      Console.WriteLine(trie.Contains("fuck"));
    }
  }
}
