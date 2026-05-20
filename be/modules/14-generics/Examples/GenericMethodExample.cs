namespace Learn.M14.Examples;

public static class GenericMethodExample
{
  public static void Run()
  {
    Console.WriteLine(First("a", "b"));
    Console.WriteLine(First(10, 20));
  }

  private static T First<T>(T a, T b)
  {
    return a;
  }
}
