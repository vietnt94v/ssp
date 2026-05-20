namespace Learn.M06.Examples;

public static class ObjectInitializerExample
{
  public static void Run()
  {
    var c = new Counter { Value = 5 };
    c.Increment();
    Console.WriteLine(c.Value);
  }

  private sealed class Counter
  {
    public int Value { get; set; }

    public void Increment()
    {
      Value++;
    }
  }
}
