using System.Collections.Generic;
using System.Runtime.InteropServices;
using TryHards.Trie;

namespace TryHards.Dictionary
{
  public static class TrieExtensions
  {
    public static IEnumerable<string> GetWordEnumerable(this DictionaryEdgeTrie<char> trie)
    {
      string Collector(List<char> wordBuffer)
      {
        return new string(CollectionsMarshal.AsSpan(wordBuffer));
      }

      DictionaryEdgeTrie<char>.EnumeratorWrapper<string> wrapper = new DictionaryEdgeTrie<char>.EnumeratorWrapper<string>
      {
        Buffer = new(),
        Collector = Collector
      };

      return wrapper.GetWordEnumeratorInternal(trie);
    }
  }
}