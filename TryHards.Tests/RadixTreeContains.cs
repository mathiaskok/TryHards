using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TryHards.Radix;

namespace TryHards.Tests
{
  [TestFixture]
  public class RadixTreeContains
  {
    public class TestSet
    {
      public string[] Patterns;
      public (string text, bool contains)[] Matches;
      public IEqualityComparer<char> Comparer;

      public override string ToString()
      {
        var pattensString = string.Join(", ", Patterns.Select(p => $"\"{p}\""));
        var matchesString = string.Join(", ", Matches.Select(m => $"(\"{m.text}\", {m.contains})"));

        return $"[{pattensString}], [{matchesString}], {Comparer?.ToString() ?? "null"}";
      }
    }

    public class OrdinalCharComparer : IEqualityComparer<char>
    {
      public bool Equals(char x, char y)
      {
        return char.ToUpper(x) == char.ToUpper(y);
      }

      public int GetHashCode([DisallowNull] char obj)
      {
        return char.ToUpper(obj).GetHashCode();
      }
    }

    public void TestBase(TestSet test)
    {
      RadixTree<char> tree = new(test.Comparer);
      foreach (var pattern in test.Patterns)
        tree.Insert(pattern.AsMemory());

      Assert.Multiple(() =>
      {
        foreach (var match in test.Matches)
          Assert.That(tree.ContainsKey(match.text.AsMemory()), Is.EqualTo(match.contains));
      });

      tree.Compact();

      Assert.Multiple(() =>
      {
        foreach (var match in test.Matches)
          Assert.That(tree.ContainsKey(match.text.AsMemory()), Is.EqualTo(match.contains));
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

        yield return new TestSet
        {
          Patterns =
          [
            "apPle",
          ],
          Matches =
          [
            ("apple", true),
            ("APPLE", true)
          ],
          Comparer = new OrdinalCharComparer()
        };
      }
    }

    [TestCaseSource(nameof(EdgeCases))]
    public void BoundaryConditions(TestSet test) => TestBase(test);
  }
}