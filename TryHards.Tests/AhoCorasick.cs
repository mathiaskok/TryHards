using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Internal;
using TryHards.AhoCorasick;

namespace TryHards.Tests
{
  [TestFixture]
  public class AhoCorasick
  {
    public class TestSet
    {
      public string[] Patterns;
      public string Text;
      public (int, string)[] ExpectedOutput;

      public override string ToString()
      {
        string patternString = string.Join(", ", Patterns.Select(p => $"\"{p}\""));
        string textString = $"\"{Text}\"";
        string outputString = string.Join(", ", ExpectedOutput.Select(o => $"({o.Item1}, \"{o.Item2}\")"));

        return $"[{patternString}], {textString}, [{outputString}]";
      }
    }

    public void TestBase(TestSet test)
    {
      Trie<char> trie = new();

      foreach (var pattern in test.Patterns)
        trie.Insert(pattern.AsMemory());

      trie.BuildAutomation();

      var result = trie.FindAllMatches(test.Text.AsMemory()).ToList();

      Assert.That(result.Count, Is.EqualTo(test.ExpectedOutput.Length));
      Assert.Multiple(() =>
      {
        for (int i = 0; i < result.Count; i++)
        {
          var res = result[i];
          var exp = test.ExpectedOutput[i];

          Assert.That(res.index, Is.EqualTo(exp.Item1));
          Assert.That(res.pattern.ToString(), Is.EqualTo(exp.Item2));
        }
      });
    }

    public static IEnumerable<TestSet> SuffixOverlapCases
    {
      get
      {
        yield return new TestSet
        {
          Patterns =
          [
            "apple",
            "ple"
          ],
          Text = "apple",
          ExpectedOutput =
          [
            (0, "apple"),
            (2, "ple")
          ]
        };
      }
    }

    [TestCaseSource(nameof(SuffixOverlapCases))]
    public void SuffixOverlap(TestSet test) => TestBase(test);

    public static IEnumerable<TestSet> PrefixOverlapCases
    {
      get
      {
        yield return new TestSet
        {
          Patterns =
          [
            "app",
            "apple"
          ],
          Text = "apple",
          ExpectedOutput =
          [
            (0, "app"),
            (0, "apple")
          ]
        };
      }
    }

    [TestCaseSource(nameof(SuffixOverlapCases))]
    public void PrefixOverlap(TestSet test) => TestBase(test);

    public static IEnumerable<TestSet> InternalOverlapCases
    {
      get
      {
        yield return new TestSet
        {
          Patterns =
          [
            "pin",
            "apple",
            "applepie"
          ],
          Text = "pineapplepie",
          ExpectedOutput =
          [
            (0, "pin"),
            (4, "apple"),
            (4, "applepie")
          ]
        };
      }
    }

    [TestCaseSource(nameof(InternalOverlapCases))]
    public void InternalOverlap(TestSet test) => TestBase(test);

    public static IEnumerable<TestSet> IdenticalSuffixesCases
    {
      get
      {
        yield return new TestSet
        {
          Patterns =
          [
            "abc",
            "bc",
            "c"
          ],
          Text = "abc",
          ExpectedOutput =
          [
            (0, "abc"),
            (1, "bc"),
            (2, "c")
          ]
        };
      }
    }

    [TestCaseSource(nameof(IdenticalSuffixesCases))]
    public void IdenticalSuffixes(TestSet test) => TestBase(test);

    public static IEnumerable<TestSet> EmptyPatternSetCases
    {
      get
      {
        yield return new TestSet
        {
          Patterns = [],
          Text = "hello",
          ExpectedOutput = []
        };
      }
    }

    [TestCaseSource(nameof(EmptyPatternSetCases))]
    public void EmptyPatternSet(TestSet test) => TestBase(test);

    public static IEnumerable<TestSet> EmptyTextCases
    {
      get
      {
        yield return new TestSet
        {
          Patterns =
          [
            "a",
            "b"
          ],
          Text = "",
          ExpectedOutput = []
        };
      }
    }

    [TestCaseSource(nameof(EmptyTextCases))]
    public void EmptyText(TestSet test) => TestBase(test);

    public static IEnumerable<TestSet> LongPatternCases
    {
      get
      {
        yield return new TestSet
        {
          Patterns =
          [
            "superlongpattern"
          ],
          Text = "short",
          ExpectedOutput = []
        };
      }
    }

    [TestCaseSource(nameof(LongPatternCases))]
    public void LongPattern(TestSet test) => TestBase(test);

    public static IEnumerable<TestSet> SingleCharacterCases
    {
      get
      {
        yield return new TestSet
        {
          Patterns =
          [
            "a",
            "b"
          ],
          Text = "abab",
          ExpectedOutput =
          [
            (0, "a"),
            (1, "b"),
            (2, "a"),
            (3, "b")
          ]
        };
      }
    }

    [TestCaseSource(nameof(SingleCharacterCases))]
    public void SingleCharacter(TestSet test) => TestBase(test);

    public static IEnumerable<TestSet> OverlappingRepeatsCases
    {
      get
      {
        yield return new TestSet
        {
          Patterns =
          [
            "aa"
          ],
          Text = "aaaa",
          ExpectedOutput =
          [
            (0, "aa"),
            (1, "aa"),
            (2, "aa")
          ]
        };
      }
    }

    [TestCaseSource(nameof(OverlappingRepeatsCases))]
    public void OverlappingRepeats(TestSet test) => TestBase(test);

    public static IEnumerable<TestSet> DisjointCharactersCases
    {
      get
      {
        yield return new TestSet
        {
          Patterns =
          [
            "a",
            "b",
            "c"
          ],
          Text = "xyz",
          ExpectedOutput = []
        };
      }
    }

    [TestCaseSource(nameof(DisjointCharactersCases))]
    public void DisjointCharacters(TestSet test) => TestBase(test);
  }
}