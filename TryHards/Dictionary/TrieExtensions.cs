using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace TryHards.Dictionary
{
  public static class TrieExtensions
  {
    public static IEnumerable<string> GetWordEnumerable(this Trie<char> trie)
    {
      string Collector(List<char> wordBuffer)
      {
        return new string(CollectionsMarshal.AsSpan(wordBuffer));
      }

      Trie<char>.EnumeratorWrapper<string> wrapper = new Trie<char>.EnumeratorWrapper<string>
      {
        Buffer = new(),
        Collector = Collector
      };

      return wrapper.GetWordEnumeratorInternal(trie);
    }
  }
}