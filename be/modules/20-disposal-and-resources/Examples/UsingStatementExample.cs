namespace Learn.M20.Examples;

public static class UsingStatementExample
{
  public static void Run()
  {
    using var temp = new TempResource();
    temp.Use();
  }

  private sealed class TempResource : IDisposable
  {
    public void Use()
    {
      Console.WriteLine("dang dung tai nguyen");
    }

    public void Dispose()
    {
      Console.WriteLine("dispose (using)");
    }
  }
}
