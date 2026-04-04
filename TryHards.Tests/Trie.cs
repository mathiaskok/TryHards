using System;
using System.Collections.Generic;
using System.Linq;
using TryHards.Dictionary;

namespace TryHards.Tests
{
  [TestFixture]
  public class TrieContains
  {
    public class TestSet
    {
      public string[] Patterns;
      public (string text, bool contains)[] Matches;

      public override string ToString()
      {
        var pattensString = string.Join(", ", Patterns.Select(p => $"\"{p}\""));
        var matchesString = string.Join(", ", Matches.Select(m => $"(\"{m.text}\", {m.contains})"));

        return $"[{pattensString}], [{matchesString}]";
      }
    }

    public void TestBase(TestSet test)
    {
      Trie<char> trie = new();

      foreach (var pattern in test.Patterns)
        trie.Insert(pattern.AsMemory().Span);

      Assert.Multiple(() =>
      {
        foreach (var match in test.Matches)
          Assert.That(trie.ContainsKey(match.text.AsMemory().Span), Is.EqualTo(match.contains));
      });
    }

    public static IEnumerable<TestSet> BaseCases
    {
      get
      {
        yield return new TestSet
        {
          Patterns =
          [
            "apple",
            "banana"
          ],
          Matches =
          [
            ("apple", true),
            ("app", false),
            ("orange", false)
          ]
        };
      }
    }

    [TestCaseSource(nameof(BaseCases))]
    public void Base(TestSet test) => TestBase(test);

    public static IEnumerable<TestSet> PrefixSplittingCases
    {
      get
      {
        yield return new TestSet
        {
          Patterns =
          [
            "cart",
            "car",
            "care"
          ],
          Matches =
          [
            ("cart", true),
            ("car", true),
            ("care", true)
          ]
        };

        yield return new TestSet
        {
          Patterns =
          [
            "test",
            "team",
            "toast"
          ],
          Matches =
          [
            ("test", true),
            ("team", true),
            ("toast", true)
          ]
        };
      }
    }

    [TestCaseSource(nameof(PrefixSplittingCases))]
    public void PrefixSplitting(TestSet test) => TestBase(test);

    public static IEnumerable<TestSet> EdgeCases
    {
      get
      {
        yield return new TestSet
        {
          Patterns = [],
          Matches =
          [
            ("anything", false)
          ]
        };

        yield return new TestSet
        {
          Patterns = [],
          Matches =
          [
            ("", false)
          ]
        };

        yield return new TestSet
        {
          Patterns =
          [
            ""
          ],
          Matches =
          [
            ("", true)
          ]
        };

        yield return new TestSet
        {
          Patterns =
          [
            "inter",
            "international"
          ],
          Matches =
          [
            ("intern", false)
          ]
        };

        yield return new TestSet
        {
          Patterns =
          [
            "apple",
            "apple"
          ],
          Matches =
          [
            ("apple",true),
            ("APPLE", false)
          ]
        };
      }
    }

    [TestCaseSource(nameof(EdgeCases))]
    public void BoundaryConditions(TestSet test) => TestBase(test);
  }
}