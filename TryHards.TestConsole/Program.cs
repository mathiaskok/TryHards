using TryHards.Radix;

namespace TryHards.TestConsole
{
  internal class Program
  {
    static void Main(string[] args)
    {
      RadixTree<char> tree = new();
      tree.Insert("cart".AsMemory());
      tree.Insert("car".AsMemory());
      tree.Insert("care".AsMemory());

      System.Console.WriteLine(tree.Contains("cart".AsMemory()));
      System.Console.WriteLine(tree.Contains("car".AsMemory()));
      System.Console.WriteLine(tree.Contains("care".AsMemory()));
    }
  }
}
