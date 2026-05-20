namespace Learn.M10.Examples;

public static class StaticExample
{
  public static void Run()
  {
    Counter.Increment();
    Counter.Increment();
    Console.WriteLine(Counter.Value);
  }

  private static class Counter
  {
    public static int Value { get; private set; }

    public static void Increment()
    {
      Value++;
    }
  }
}
