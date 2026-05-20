namespace Learn.M11.Examples;

public static class IEnumerableExample
{
  public static void Run()
  {
    foreach (var n in CountUp(3))
    {
      Console.Write(n + " ");
    }

    Console.WriteLine();
  }

  private static IEnumerable<int> CountUp(int max)
  {
    for (var i = 1; i <= max; i++)
    {
      yield return i;
    }
  }
}
