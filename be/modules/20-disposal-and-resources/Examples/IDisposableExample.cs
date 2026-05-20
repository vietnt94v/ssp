namespace Learn.M20.Examples;

public static class IDisposableExample
{
  public static void Run()
  {
    using (var r = new CounterResource())
    {
      r.Tick();
    }
  }

  private sealed class CounterResource : IDisposable
  {
    private int count;
    private bool disposed;

    public void Tick()
    {
      count++;
      Console.WriteLine($"tick {count}");
    }

    public void Dispose()
    {
      if (disposed)
      {
        return;
      }

      disposed = true;
      Console.WriteLine("CounterResource disposed");
    }
  }
}
